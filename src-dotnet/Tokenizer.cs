/*
 * Tokenizer.java
 */
using Java.Io;

namespace Nsl
{
    public class Tokenizer : StreamTokenizer
    {
        private readonly Reader theReader;
        private readonly string source;
        private int lineNumberAdd;
        private bool autoPop;
        public Tokenizer(Reader reader, string source) : base(reader)
        {
            this.ResetSyntax();
            this.SlashSlashComments(true);
            this.SlashStarComments(true);
            this.WordChars('a', 'z');
            this.WordChars('A', 'Z');
            this.WordChars('0', '9');
            this.WordChars(160, 255);
            this.WordChars('_', '_');
            this.WordChars('$', '$');
            this.WordChars('#', '#');
            this.WhitespaceChars(0, 32);

            //this.ordinaryChar('.');
            //this.ordinaryChar('/');
            //this.ordinaryChar('-');
            this.QuoteChar('"');
            this.QuoteChar('\'');
            this.QuoteChar('`');
            this.theReader = reader;
            this.source = source;
            this.lineNumberAdd = 0;
            this.autoPop = true;
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        public virtual Reader GetReader()
        {
            return this.theReader;
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        public virtual string GetSource()
        {
            return this.source;
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        public override int Lineno()
        {
            return base.Lineno() + this.lineNumberAdd;
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        public virtual void SetAutoPop(bool autoPop)
        {
            this.autoPop = autoPop;
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        public virtual bool TokenIs(string word)
        {
            return this.TokenIsWord() && this.sval.Equals(word);
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        public virtual bool TokenIs(char c)
        {
            return (char)this.ttype == c;
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        public virtual bool TokenIsChar()
        {
            return !TokenIsWord() && !TokenIsNumber() && !TokenIsString();
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        public virtual bool TokenIsWord()
        {
            return this.ttype == TT_WORD;
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        public virtual bool TokenIsNumber()
        {
            return this.ttype == TT_NUMBER;
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        public virtual bool TokenIsString()
        {
            return this.ttype == '"' || this.ttype == '\'' || this.ttype == '`';
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        public virtual string ReadUntil(string match)
        {
            char[] matchChars = match.ToCharArray();
            string text = "";
            try
            {
                int c = 0;
                while (true)
                {
                    int b = this.theReader.Read();
                    if (b == -1)
                    {
                        this.ttype = TT_EOF;
                        throw new NslExpectedException(match);
                    }

                    if (b == '\r')
                    {
                        this.lineNumberAdd++;
                        if (!text.IsEmpty())
                            text += '\n';
                        b = this.theReader.Read();
                        continue;
                    }

                    if (b == '\n')
                    {
                        this.lineNumberAdd++;
                        if (!text.IsEmpty())
                            text += '\n';
                        continue;
                    }

                    if (b == matchChars[c])
                    {
                        c++;
                        if (c == matchChars.length)
                        {
                            text = text.Substring(0, text.Length() - matchChars.length + 1);
                            break;
                        }
                    }
                    else
                        c = 0;
                    text += (char)b;
                }
            }
            catch (IOException ex)
            {
                throw new NslException(ex.GetMessage(), true);
            }

            return text;
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        public virtual bool TokenNext()
        {
            return this.TokenNext(null);
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        public virtual bool TokenNext(string expected)
        {
            try
            {
                bool result = this.NextToken() != TT_EOF;
                if (!result)
                {
                    if (this.autoPop && ScriptParser.PopTokenizer() != null)
                        return true;
                    if (expected != null)
                        throw new NslExpectedException(expected);
                }


                // We translate numbers ourself.
                if (this.ttype == TT_WORD)
                {
                    char c = this.sval.CharAt(0);
                    if (c >= '0' && c <= '9')
                    {
                        this.ttype = TT_NUMBER;
                        try
                        {
                            if (this.sval.Length() > 1 && this.sval.StartsWith("0x"))
                            {
                                this.nval = Integer.ParseInt(this.sval.Substring(2), 16);
                            }
                            else
                            {
                                this.nval = Integer.ParseInt(this.sval);
                            }
                        }
                        catch (NumberFormatException ex)
                        {
                            throw new NslException(ex.GetMessage(), true);
                        }
                    }
                }


                // String with no escape sequences.
                if (this.ttype == '@')
                {
                    int b = this.theReader.Read();
                    if (b == -1)
                    {
                        this.ttype = TT_EOF;
                        throw new NslExpectedException("a string");
                    }

                    this.ttype = (char)b;
                    if (b != '"' && b != '\'' && b != '`')
                    {
                        throw new NslExpectedException("a string");
                    }

                    this.sval = ReadUntil(String.ValueOf((char)b));
                }

                return result;
            }
            catch (IOException ex)
            {
                throw expected == null ? new NslException(ex.GetMessage(), true) : new NslExpectedException(expected);
            }
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        // We translate numbers ourself.
        // String with no escape sequences.
        /*public String tokenToString()
  {
    if (this.tokenIsWord())
      return this.sval;
    if (this.tokenIsNumber())
      return Integer.toString((int)this.nval);
    if (this.tokenIsString())
      return '"' + this.sval + '"';
    return Character.toString((char)this.ttype);
  }*/
        public virtual void MatchOrDie(char c)
        {
            if (!this.TokenIs(c))
                throw new NslExpectedException("\"" + c + "\"");
            this.TokenNext();
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        // We translate numbers ourself.
        // String with no escape sequences.
        /*public String tokenToString()
  {
    if (this.tokenIsWord())
      return this.sval;
    if (this.tokenIsNumber())
      return Integer.toString((int)this.nval);
    if (this.tokenIsString())
      return '"' + this.sval + '"';
    return Character.toString((char)this.ttype);
  }*/
        public virtual void MatchOrDie(string word)
        {
            if (!this.TokenIs(word))
                throw new NslExpectedException("\"" + word + "\"");
            this.TokenNext();
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        // We translate numbers ourself.
        // String with no escape sequences.
        /*public String tokenToString()
  {
    if (this.tokenIsWord())
      return this.sval;
    if (this.tokenIsNumber())
      return Integer.toString((int)this.nval);
    if (this.tokenIsString())
      return '"' + this.sval + '"';
    return Character.toString((char)this.ttype);
  }*/
        public virtual bool Match(char c)
        {
            if (this.TokenIs(c))
            {
                this.TokenNext();
                return true;
            }

            return false;
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        // We translate numbers ourself.
        // String with no escape sequences.
        /*public String tokenToString()
  {
    if (this.tokenIsWord())
      return this.sval;
    if (this.tokenIsNumber())
      return Integer.toString((int)this.nval);
    if (this.tokenIsString())
      return '"' + this.sval + '"';
    return Character.toString((char)this.ttype);
  }*/
        public virtual bool Match(string word)
        {
            if (this.TokenIs(word))
            {
                this.TokenNext();
                return true;
            }

            return false;
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        // We translate numbers ourself.
        // String with no escape sequences.
        /*public String tokenToString()
  {
    if (this.tokenIsWord())
      return this.sval;
    if (this.tokenIsNumber())
      return Integer.toString((int)this.nval);
    if (this.tokenIsString())
      return '"' + this.sval + '"';
    return Character.toString((char)this.ttype);
  }*/
        public virtual void MatchEolOrDie()
        {
            if (!this.TokenIs(';'))
                throw new NslExpectedException("\";\"");
            this.TokenNext();
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        // We translate numbers ourself.
        // String with no escape sequences.
        /*public String tokenToString()
  {
    if (this.tokenIsWord())
      return this.sval;
    if (this.tokenIsNumber())
      return Integer.toString((int)this.nval);
    if (this.tokenIsString())
      return '"' + this.sval + '"';
    return Character.toString((char)this.ttype);
  }*/
        public virtual string MatchAWord()
        {
            return this.MatchAWord(null);
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        // We translate numbers ourself.
        // String with no escape sequences.
        /*public String tokenToString()
  {
    if (this.tokenIsWord())
      return this.sval;
    if (this.tokenIsNumber())
      return Integer.toString((int)this.nval);
    if (this.tokenIsString())
      return '"' + this.sval + '"';
    return Character.toString((char)this.ttype);
  }*/
        public virtual string MatchAWord(string expected)
        {
            if (this.TokenIsWord())
            {
                string word = this.sval;
                this.TokenNext();
                return word;
            }

            if (expected != null)
                throw new NslExpectedException(expected);
            return null;
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        // We translate numbers ourself.
        // String with no escape sequences.
        /*public String tokenToString()
  {
    if (this.tokenIsWord())
      return this.sval;
    if (this.tokenIsNumber())
      return Integer.toString((int)this.nval);
    if (this.tokenIsString())
      return '"' + this.sval + '"';
    return Character.toString((char)this.ttype);
  }*/
        public virtual string MatchAString()
        {
            return this.MatchAString(null);
        }

        //this.ordinaryChar('.');
        //this.ordinaryChar('/');
        //this.ordinaryChar('-');
        // We translate numbers ourself.
        // String with no escape sequences.
        /*public String tokenToString()
  {
    if (this.tokenIsWord())
      return this.sval;
    if (this.tokenIsNumber())
      return Integer.toString((int)this.nval);
    if (this.tokenIsString())
      return '"' + this.sval + '"';
    return Character.toString((char)this.ttype);
  }*/
        public virtual string MatchAString(string expected)
        {
            if (this.TokenIsString())
            {
                string string = this.sval;
                this.TokenNext();
                return @string;
            }

            if (expected != null)
                throw new NslExpectedException(expected);
            return null;
        }
    }
}