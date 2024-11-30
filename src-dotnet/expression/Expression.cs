/*
 * Expression.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Expression
{
    public class Expression
    {
        protected ExpressionType type;
        protected string stringValue;
        protected int integerValue;
        protected bool booleanValue;
        public static readonly Expression Empty = Expression.FromString("");
        public static readonly Expression Null = new Expression();
        private static bool specialStringEscape = true;
        public static bool SetSpecialStringEscape(bool value)
        {
            bool oldValue = specialStringEscape;
            specialStringEscape = value;
            return oldValue;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        protected Expression()
        {
            this.type = ExpressionType.Other;
        }

        private Expression(Tokenizer tokenizer)
        {
            if (tokenizer.TokenIsWord())
            {
                if (tokenizer.sval.StartsWith("$"))
                {
                    if (tokenizer.sval.Length() == 1)
                    {
                        tokenizer.TokenNext();
                        if (tokenizer.Match('('))
                        {
                            this.type = ExpressionType.String;
                            this.stringValue = "$(";
                            if (tokenizer.Match('^'))
                                this.stringValue += "^";
                            this.stringValue += tokenizer.MatchAWord("a language string");
                            if (!tokenizer.TokenIs(')'))
                                throw new NslException("Register, constant or language string requires a name", true);
                            this.stringValue += ")";
                            this.booleanValue = false;
                            this.integerValue = 0;
                        }
                        else
                            throw new NslException("Register, constant or language string requires a name", true);
                    }
                    else
                    {
                        int constant = ConstantList.GetCurrent().Lookup(tokenizer.sval);
                        if (constant != -1)
                        {
                            this.type = ExpressionType.Constant;
                            this.stringValue = null;
                            this.booleanValue = false;
                            this.integerValue = constant;
                        }
                        else
                        {
                            this.type = ExpressionType.Register;
                            this.stringValue = null;
                            this.booleanValue = false;
                            this.integerValue = RegisterList.GetCurrent().Add(tokenizer.sval);
                            if (CodeInfo.GetCurrent() != null)
                                CodeInfo.GetCurrent().AddUsedVar(RegisterList.GetCurrent()[this.integerValue]);
                        }
                    }
                }
                else if (tokenizer.sval.EqualsIgnoreCase("true"))
                {
                    this.type = ExpressionType.Boolean;
                    this.stringValue = null;
                    this.booleanValue = true;
                    this.integerValue = 0;
                }
                else if (tokenizer.sval.EqualsIgnoreCase("false"))
                {
                    this.type = ExpressionType.Boolean;
                    this.stringValue = null;
                    this.booleanValue = false;
                    this.integerValue = 0;
                }
                else
                    throw new NslException("Unrecognised token \"" + tokenizer.sval + "\"", true);
            }
            else if (tokenizer.TokenIsString())
            {
                if (tokenizer.ttype == '`')
                {
                    this.type = ExpressionType.StringSpecial;
                    if (specialStringEscape)
                    {
                        this.stringValue = EscapeString(tokenizer.sval);
                        this.booleanValue = false;
                    }
                    else
                    {
                        this.stringValue = tokenizer.sval;
                        this.booleanValue = true;
                    }
                }
                else
                {
                    this.type = ExpressionType.String;
                    this.stringValue = EscapeString(tokenizer.sval);
                    this.booleanValue = false;
                }

                this.integerValue = 0;
            }
            else if (tokenizer.TokenIsNumber())
            {
                this.type = ExpressionType.Integer;
                this.integerValue = (int)tokenizer.nval;
                this.stringValue = null;
                this.booleanValue = false;
            }
            else
                throw new NslException("Unrecognised token \"" + (char)tokenizer.ttype + "\"", true);
            tokenizer.TokenNext();
        }

        private static string EscapeString(string escape)
        {
            return escape.ReplaceAll("\\$", "\\$\\$").ReplaceAll("\"", "\\$\\\\\"").ReplaceAll("\r", "\\$\\\\r").ReplaceAll("\n", "\\$\\\\n").ReplaceAll("\t", "\\$\\\\t");
        }

        public static Expression FromRegister(string register)
        {
            Expression expression = new Expression();
            expression.type = ExpressionType.Register;
            expression.stringValue = null;
            expression.booleanValue = false;
            expression.integerValue = RegisterList.GetCurrent().Add(register);
            return expression;
        }

        public static Expression FromString(string value)
        {
            Expression expression = new Expression();
            expression.type = ExpressionType.String;
            expression.stringValue = value;
            expression.booleanValue = false;
            expression.integerValue = 0;
            return expression;
        }

        public static Expression FromSpecialString(string value)
        {
            Expression expression = new Expression();
            expression.type = ExpressionType.StringSpecial;
            expression.stringValue = value;
            expression.booleanValue = !specialStringEscape;
            expression.integerValue = 0;
            return expression;
        }

        public static Expression FromInteger(int value)
        {
            Expression expression = new Expression();
            expression.type = ExpressionType.Integer;
            expression.stringValue = null;
            expression.booleanValue = false;
            expression.integerValue = value;
            return expression;
        }

        public static Expression FromBoolean(bool value)
        {
            Expression expression = new Expression();
            expression.type = ExpressionType.Boolean;
            expression.stringValue = null;
            expression.booleanValue = value;
            expression.integerValue = 0;
            return expression;
        }

        public virtual bool IsLiteral()
        {
            return true;
        }

        private static Expression CreateMathematical(Expression left, string @operator, Expression right)
        {

            // Matched registers. Check their scope.
            if (left.type.Equals(ExpressionType.Register))
                Scope.GetCurrent().Check(left.integerValue);
            if (right.type.Equals(ExpressionType.Register))
                Scope.GetCurrent().Check(right.integerValue);

            // Left and right are literals.
            if (left.IsLiteral() && right.IsLiteral())
            {

                // Should not be using mathematical operators on Boolean values.
                if (left.type.Equals(ExpressionType.Boolean) || right.type.Equals(ExpressionType.Boolean))
                    NslException.PrintWarning("\"" + @operator + "\" operator used with a Boolean operand");

                // Should not be using mathematical operators on string values.
                if (ExpressionType.IsString(left) || ExpressionType.IsString(right))
                    NslException.PrintWarning("\"" + @operator + "\" operator used with a string operand");

                // Integer types.
                if (left.type.Equals(ExpressionType.Integer) && right.type.Equals(ExpressionType.Integer))
                {
                    if (@operator.Equals("+"))
                        return Expression.FromInteger(left.integerValue + right.integerValue);
                    if (@operator.Equals("-"))
                        return Expression.FromInteger(left.integerValue - right.integerValue);
                    if (@operator.Equals("*"))
                        return Expression.FromInteger(left.integerValue * right.integerValue);
                    if (@operator.Equals("/"))
                    {
                        if (right.integerValue == 0)
                            throw new NslException("Division by zero", true);
                        return Expression.FromInteger(left.integerValue / right.integerValue);
                    }

                    if (@operator.Equals("%"))
                        return Expression.FromInteger(left.integerValue % right.integerValue);
                    if (@operator.Equals("|"))
                        return Expression.FromInteger(left.integerValue | right.integerValue);
                    if (@operator.Equals("&"))
                        return Expression.FromInteger(left.integerValue & right.integerValue);
                    if (@operator.Equals("^"))
                        return Expression.FromInteger(left.integerValue ^ right.integerValue);
                    if (@operator.Equals("<<"))
                        return Expression.FromInteger(left.integerValue << right.integerValue);
                    if (@operator.Equals(">>"))
                        return Expression.FromInteger(left.integerValue >> right.integerValue);
                    if (@operator.Equals("~"))
                        return Expression.FromInteger(~left.integerValue);
                }
            }

            return new MathematicalExpression(left, @operator, right);
        }

        private static Expression CreateComparison(Expression left, string @operator, Expression right, ComparisonType comparisonType)
        {

            // Matched registers. Check their scope.
            if (left.type.Equals(ExpressionType.Register))
                Scope.GetCurrent().Check(left.integerValue);
            if (right.type.Equals(ExpressionType.Register))
                Scope.GetCurrent().Check(right.integerValue);

            // Left and right are literals.
            if (left.IsLiteral() && right.IsLiteral())
            {
                if (left.type.Equals(ExpressionType.Boolean) || right.type.Equals(ExpressionType.Boolean))
                {

                    // Should not be using these operators on Boolean values.
                    if (@operator.Equals(">") || @operator.Equals(">=") || @operator.Equals("<") || @operator.Equals("<="))
                        NslException.PrintWarning("\"" + @operator + "\" operator used with a Boolean operand");
                    else
                        comparisonType = ComparisonType.String;
                }


                // Integer types.
                if (left.type.Equals(ExpressionType.Integer) && right.type.Equals(ExpressionType.Integer))
                {
                    if (comparisonType.Equals(ComparisonType.IntegerUnsigned))
                    {
                        left.integerValue = Math.Abs(left.integerValue);
                        right.integerValue = Math.Abs(right.integerValue);
                    }

                    if (@operator.Equals("=="))
                        return Expression.FromBoolean(left.integerValue == right.integerValue);
                    if (@operator.Equals("!="))
                        return Expression.FromBoolean(left.integerValue != right.integerValue);
                    if (@operator.Equals(">"))
                        return Expression.FromBoolean(left.integerValue > right.integerValue);
                    if (@operator.Equals(">="))
                        return Expression.FromBoolean(left.integerValue >= right.integerValue);
                    if (@operator.Equals("<"))
                        return Expression.FromBoolean(left.integerValue < right.integerValue);
                    if (@operator.Equals("<="))
                        return Expression.FromBoolean(left.integerValue <= right.integerValue);
                }
                else 
// Boolean types.
                if (left.type.Equals(ExpressionType.Boolean) && right.type.Equals(ExpressionType.Boolean))
                {
                    if (@operator.Equals("=="))
                        return Expression.FromBoolean(left.booleanValue == right.booleanValue);
                    if (@operator.Equals("!="))
                        return Expression.FromBoolean(left.booleanValue != right.booleanValue);
                }
                else 
// String types.
                if (ExpressionType.IsString(left) && ExpressionType.IsString(right))
                {
                    if (comparisonType.Equals(ComparisonType.StringCaseSensitive))
                    {
                        if (@operator.Equals("=="))
                            return Expression.FromBoolean(left.ToString(true).Equals(right.ToString(true)));
                        if (@operator.Equals("!="))
                            return Expression.FromBoolean(!left.ToString(true).Equals(right.ToString(true)));
                        if (@operator.Equals(">"))
                            return Expression.FromBoolean(left.ToString(true).CompareTo(right.ToString(true)) > 0);
                        if (@operator.Equals(">="))
                            return Expression.FromBoolean(left.ToString(true).CompareTo(right.ToString(true)) >= 0);
                        if (@operator.Equals("<"))
                            return Expression.FromBoolean(left.ToString(true).CompareTo(right.ToString(true)) < 0);
                        if (@operator.Equals("<="))
                            return Expression.FromBoolean(left.ToString(true).CompareTo(right.ToString(true)) <= 0);
                    }
                    else
                    {
                        if (@operator.Equals("=="))
                            return Expression.FromBoolean(left.ToString(true).EqualsIgnoreCase(right.ToString(true)));
                        if (@operator.Equals("!="))
                            return Expression.FromBoolean(!left.ToString(true).EqualsIgnoreCase(right.ToString(true)));
                        if (@operator.Equals(">"))
                            return Expression.FromBoolean(left.ToString(true).CompareToIgnoreCase(right.ToString(true)) > 0);
                        if (@operator.Equals(">="))
                            return Expression.FromBoolean(left.ToString(true).CompareToIgnoreCase(right.ToString(true)) >= 0);
                        if (@operator.Equals("<"))
                            return Expression.FromBoolean(left.ToString(true).CompareToIgnoreCase(right.ToString(true)) < 0);
                        if (@operator.Equals("<="))
                            return Expression.FromBoolean(left.ToString(true).CompareToIgnoreCase(right.ToString(true)) <= 0);
                    }
                }
            }
            else if (ExpressionType.IsBoolean(left))
            {
                if (@operator.Equals("==") && right.type == ExpressionType.Boolean)
                {
                    if (left.booleanValue == false)
                        right.booleanValue = !right.booleanValue;
                    return right;
                }

                if (@operator.Equals("!=") && right.type == ExpressionType.Boolean)
                {
                    if (left.booleanValue == true)
                        right.booleanValue = !right.booleanValue;
                    return right;
                }

                comparisonType = ComparisonType.String;
            }
            else if (ExpressionType.IsBoolean(right))
            {
                if (@operator.Equals("==") && left.type == ExpressionType.Boolean)
                {
                    if (right.booleanValue == false)
                        left.booleanValue = !left.booleanValue;
                    return left;
                }

                if (@operator.Equals("!=") && left.type == ExpressionType.Boolean)
                {
                    if (right.booleanValue == true)
                        left.booleanValue = !left.booleanValue;
                    return left;
                }

                comparisonType = ComparisonType.String;
            }

            return new ComparisonExpression(left, @operator, right, comparisonType);
        }

        private static Expression CreateBoolean(Expression left, string @operator, Expression right)
        {

            // Matched registers. Check their scope.
            if (left.type.Equals(ExpressionType.Register))
                Scope.GetCurrent().Check(left.integerValue);
            if (right.type.Equals(ExpressionType.Register))
                Scope.GetCurrent().Check(right.integerValue);

            // Left and right are literals. If both are also Boolean, we can evaluate
            // now.
            if (left.IsLiteral() && right.IsLiteral())
            {
                if (left.type.Equals(ExpressionType.Boolean) && right.type.Equals(ExpressionType.Boolean))
                {
                    if (@operator.Equals("&&"))
                        return Expression.FromBoolean(left.booleanValue && right.booleanValue);
                    if (@operator.Equals("||"))
                        return Expression.FromBoolean(left.booleanValue || right.booleanValue);
                }
            }
            else if (right.IsLiteral())
            {

                // If the right operand is a literal true and the operator is && then we can
                // discard the right operand.
                if (@operator.Equals("&&") && right.type.Equals(ExpressionType.Boolean) && right.booleanValue == true)
                    return left;

                // If the right operand is a literal false and the operator is || then we
                // can discard the right operand.
                if (@operator.Equals("||") && right.type.Equals(ExpressionType.Boolean) && right.booleanValue == false)
                    return left;
            }

            return new BooleanExpression(left, @operator, right);
        }

        private static Expression CreateConcatenation(Expression left, Expression right)
        {

            // Matched registers. Check their scope.
            if (left.type.Equals(ExpressionType.Register))
                Scope.GetCurrent().Check(left.integerValue);
            if (right.type.Equals(ExpressionType.Register))
                Scope.GetCurrent().Check(right.integerValue);

            // Left and right are literals; concatenate them.
            if (left.IsLiteral() && right.IsLiteral())
            {
                if (left.type.Equals(ExpressionType.StringSpecial) || right.type.Equals(ExpressionType.StringSpecial))
                    return Expression.FromSpecialString(left.ToString(true) + right.ToString(true));
                return Expression.FromString(left.ToString(true) + right.ToString(true));
            }

            return new ConcatenationExpression(left, right);
        }

        private static Expression CreateTernary(Expression left, Expression ifTrue, Expression ifFalse)
        {

            // Matched registers. Check their scope.
            if (left.type.Equals(ExpressionType.Register))
                Scope.GetCurrent().Check(left.integerValue);
            if (ifTrue.type.Equals(ExpressionType.Register))
                Scope.GetCurrent().Check(ifTrue.integerValue);
            if (ifFalse.type.Equals(ExpressionType.Register))
                Scope.GetCurrent().Check(ifFalse.integerValue);
            if (left.IsLiteral())
            {
                if (left.type.Equals(ExpressionType.Boolean))
                {
                    if (left.booleanValue == true)
                        return ifTrue;
                    return ifFalse;
                }
            }

            return new TernaryExpression(left, ifTrue, ifFalse);
        }

        private static Expression MatchPrimary()
        {
            bool logicalNegate = false, binaryNegate = false, minus = false;

            // The expression is prefixed with a char.
            if (ScriptParser.tokenizer.TokenIsChar())
            {

                // Logical negate the expression.
                if (ScriptParser.tokenizer.Match('!'))
                    logicalNegate = true;
                else 
// Binary negate the expression.
                if (ScriptParser.tokenizer.Match('~'))
                    binaryNegate = true;

                // Negative number.
                if (ScriptParser.tokenizer.Match('-'))
                    minus = true;

                // New bracket; new expression.
                if (ScriptParser.tokenizer.Match('('))
                {
                    Expression left = MatchComplex();

                    // Negate the returned Boolean (logical) expression.
                    if (logicalNegate)
                    {
                        if (!left.type.Equals(ExpressionType.Boolean))
                            throw new NslException("The \"!\" operator must be applied to a Boolean expression", true);
                        /*if (left instanceof ConditionalExpression)
            ((ConditionalExpression)left).setNegate(true);
          else */
                        if (left.booleanValue)
                            left.booleanValue = false;
                        else
                            left.booleanValue = true;
                    }
                    else 
// Binary negate the returned expression.
                    if (binaryNegate)
                    {
                        left = CreateMathematical(left, "~", Expression.FromInteger(0));
                    }


                    // To make the returned number negative, multiply by -1.
                    if (minus)
                    {
                        left = CreateMathematical(Expression.FromInteger(-1), "*", left);
                    }


                    // Match the end bracket.
                    ScriptParser.tokenizer.MatchOrDie(')');
                    return left;
                }


                // Would only occur if some invalid character appeared.
                if (!logicalNegate && !binaryNegate && !minus)
                    throw new NslExpectedException("an expression");
            }


            // Match the actual value.
            Expression left = Match();

            // Negate the returned Boolean (logical) value.
            if (logicalNegate)
            {
                if (!left.type.Equals(ExpressionType.Boolean))
                    throw new NslException("The \"!\" operator must be applied to a Boolean expression", true);
                if (left.booleanValue)
                    left.booleanValue = false;
                else
                    left.booleanValue = true;
            }
            else 
// Binary negate the returned value.
            if (binaryNegate)
            {
                left = CreateMathematical(left, "~", Expression.FromInteger(0));
            }


            // To make the returned number negative, multiply by -1.
            if (minus)
            {
                left = CreateMathematical(Expression.FromInteger(-1), "*", left);
            }


            // Next token is a word? It could be an operator in a late evaluation
            // constant.
            if (ScriptParser.tokenizer.TokenIsWord())
            {
                Expression value = DefineList.Lookup(ScriptParser.tokenizer.sval);
                if (value != null && value.type.Equals(ExpressionType.StringSpecial) && value.booleanValue == true)
                {
                    string name = ScriptParser.tokenizer.sval;
                    ScriptParser.tokenizer.TokenNext(); // Discard the constant name.
                    ScriptParser.PushTokenizer(new Tokenizer(new StringReader(value.stringValue), "constant \"" + name + "\""));
                }
            }

            return left;
        }

        private static Expression MatchConcatenation()
        {
            Expression left = MatchPrimary();
            while (ScriptParser.tokenizer.Match('.'))
            {

                // Match .= (assignment)
                if (ScriptParser.tokenizer.Match('='))
                {
                    if (!left.type.Equals(ExpressionType.Register))
                        throw new NslException("The left operand must be a variable", true);
                    left = new AssignmentExpression(left.integerValue, CreateConcatenation(left, MatchComplex()));
                    Scope.GetCurrent().AddVar(left.integerValue);
                }
                else

                // Matched .
                {
                    left = CreateConcatenation(left, MatchPrimary());
                }
            }

            return left;
        }

        private static Expression MatchMultiplicative()
        {
            Expression left = MatchConcatenation();
            while (ScriptParser.tokenizer.TokenIs('*') || ScriptParser.tokenizer.TokenIs('/') || ScriptParser.tokenizer.TokenIs('%'))
            {
                string operator = Character.ToString((char)ScriptParser.tokenizer.ttype);
                ScriptParser.tokenizer.TokenNext("an expression");

                // Match *=, /= or %= (assignment)
                if (ScriptParser.tokenizer.Match('='))
                {
                    if (!left.type.Equals(ExpressionType.Register))
                        throw new NslException("The left operand must be a variable", true);
                    left = new AssignmentExpression(left.integerValue, CreateMathematical(left, @operator, MatchComplex()));
                    Scope.GetCurrent().AddVar(left.integerValue);
                }
                else

                // Matched *, / or %
                {
                    left = CreateMathematical(left, @operator, MatchConcatenation());
                }
            }

            return left;
        }

        private static Expression MatchAdditive()
        {
            Expression left = MatchMultiplicative();
            while (ScriptParser.tokenizer.TokenIs('+') || ScriptParser.tokenizer.TokenIs('-'))
            {
                string operator = Character.ToString((char)ScriptParser.tokenizer.ttype);
                ScriptParser.tokenizer.TokenNext("an expression");

                // Match n++ or n--
                if (ScriptParser.tokenizer.Match(@operator.CharAt(0)))
                {
                    if (!left.type.Equals(ExpressionType.Register))
                        throw new NslException("The left operand must be a variable", true);
                    left = new AssignmentExpression(left.integerValue, CreateMathematical(left, @operator, Expression.FromInteger(1)));
                    Scope.GetCurrent().AddVar(left.integerValue);
                }
                else 
// Match += or -= (assignment)
                if (ScriptParser.tokenizer.Match('='))
                {
                    if (!left.type.Equals(ExpressionType.Register))
                        throw new NslException("The left operand must be a variable", true);
                    left = new AssignmentExpression(left.integerValue, CreateMathematical(left, @operator, MatchComplex()));
                    Scope.GetCurrent().AddVar(left.integerValue);
                }
                else

                // Matched + or -
                {
                    left = CreateMathematical(left, @operator, MatchMultiplicative());
                }
            }

            return left;
        }

        private static Expression MatchRelational()
        {
            Expression left = MatchAdditive();
            while (ScriptParser.tokenizer.TokenIs('<') || ScriptParser.tokenizer.TokenIs('>'))
            {
                string operator = Character.ToString((char)ScriptParser.tokenizer.ttype);
                ScriptParser.tokenizer.TokenNext("an expression");

                // Match <= or >=
                if (ScriptParser.tokenizer.Match('='))
                {
                    ComparisonType comparisonType = ComparisonType.Match();
                    left = CreateComparison(left, @operator + '=', MatchAdditive(), comparisonType);
                }
                else 
// Match <<
                if (@operator.Equals("<") && ScriptParser.tokenizer.Match('<'))
                {

                    // Match <<= (assignment)
                    if (ScriptParser.tokenizer.Match('='))
                    {
                        if (!left.type.Equals(ExpressionType.Register))
                            throw new NslException("The left operand must be a variable", true);
                        left = new AssignmentExpression(left.integerValue, CreateMathematical(left, "<<", MatchComplex()));
                        Scope.GetCurrent().AddVar(left.integerValue);
                    }
                    else

                    // Matched <<
                    {
                        left = CreateMathematical(left, "<<", MatchAdditive());
                    }
                }
                else 
// Match >>
                if (@operator.Equals(">") && ScriptParser.tokenizer.Match('>'))
                {

                    // Match >>= (assignment)
                    if (ScriptParser.tokenizer.Match('='))
                    {
                        if (!left.type.Equals(ExpressionType.Register))
                            throw new NslException("The left operand must be a variable", true);
                        left = new AssignmentExpression(left.integerValue, CreateMathematical(left, ">>", MatchComplex()));
                        Scope.GetCurrent().AddVar(left.integerValue);
                    }
                    else

                    // Matched >>
                    {
                        left = CreateMathematical(left, ">>", MatchAdditive());
                    }
                }
                else

                // Matched < or >
                {
                    ComparisonType comparisonType = ComparisonType.Match();
                    left = CreateComparison(left, @operator, MatchAdditive(), comparisonType);
                }
            }

            return left;
        }

        private static Expression MatchEqualityOrAssignment()
        {
            Expression left = MatchRelational();
            while (ScriptParser.tokenizer.TokenIs('!') || ScriptParser.tokenizer.TokenIs('='))
            {
                string operator = Character.ToString((char)ScriptParser.tokenizer.ttype);
                ScriptParser.tokenizer.TokenNext("an expression");

                // Match != or ==
                if (ScriptParser.tokenizer.Match('='))
                {
                    @operator += "=";
                    ComparisonType comparisonType = ComparisonType.Match();
                    left = CreateComparison(left, @operator, MatchRelational(), comparisonType);

                    // Optimise jump instructions to assemble jumps according to the Boolean
                    // expression.
                    if (left is ComparisonExpression)
                    {
                        ComparisonExpression leftComparisonExpression = (ComparisonExpression)left;
                        if (leftComparisonExpression.GetLeftOperand() is JumpExpression && leftComparisonExpression.GetRightOperand().IsLiteral())
                        {
                            JumpExpression jumpInstruction = (JumpExpression)(leftComparisonExpression).GetLeftOperand();
                            if (jumpInstruction.Optimise((leftComparisonExpression).GetRightOperand(), @operator))
                            {
                                if (leftComparisonExpression.booleanValue)
                                    jumpInstruction.booleanValue = !jumpInstruction.booleanValue;
                                left = jumpInstruction;
                            }
                        }
                        else if (leftComparisonExpression.GetRightOperand() is JumpExpression && leftComparisonExpression.GetLeftOperand().IsLiteral())
                        {
                            JumpExpression jumpInstruction = (JumpExpression)(leftComparisonExpression).GetRightOperand();
                            if (jumpInstruction.Optimise((leftComparisonExpression).GetLeftOperand(), @operator))
                            {
                                if (leftComparisonExpression.booleanValue)
                                    jumpInstruction.booleanValue = !jumpInstruction.booleanValue;
                                left = jumpInstruction;
                            }
                        }
                    }
                }
                else 
// Match = (assignment)
                if (@operator.Equals("="))
                {
                    if (!left.type.Equals(ExpressionType.Register))
                        throw new NslException("The left operand must be a variable", true);
                    left = new AssignmentExpression(left.integerValue, MatchComplex());
                    Scope.GetCurrent().AddVar(left.integerValue);
                }
            }

            return left;
        }

        private static Expression MatchLogicalAndOrBinaryAnd()
        {
            Expression left = MatchEqualityOrAssignment();
            while (ScriptParser.tokenizer.Match('&'))
            {

                // Match &&
                if (ScriptParser.tokenizer.Match('&'))
                {
                    if (ExpressionType.IsBoolean(left) && left.booleanValue == false)
                    {

                        // Evaluated to false; we need not evaluate anything up until the next
                        // ||, ) or ;.
                        while (ScriptParser.tokenizer.TokenNext("\")\""))
                        {
                            if (ScriptParser.tokenizer.TokenIs('('))
                                while (ScriptParser.tokenizer.TokenNext("\")\""))
                                    if (ScriptParser.tokenizer.Match(')'))
                                        break;
                            if (ScriptParser.tokenizer.TokenIs(')') || ScriptParser.tokenizer.TokenIs('|') || ScriptParser.tokenizer.TokenIs(';'))
                                break;
                        }
                    }
                    else
                    {
                        left = CreateBoolean(left, "&&", MatchEqualityOrAssignment());
                    }
                }
                else 
// Match &= (assignment)
                if (ScriptParser.tokenizer.Match('='))
                {
                    if (!left.type.Equals(ExpressionType.Register))
                        throw new NslException("The left operand must be a variable", true);
                    left = new AssignmentExpression(left.integerValue, CreateMathematical(left, "&", MatchComplex()));
                    Scope.GetCurrent().AddVar(left.integerValue);
                }
                else

                // Matched &
                {
                    left = CreateMathematical(left, "&", MatchEqualityOrAssignment());
                }
            }

            return left;
        }

        private static Expression MatchBinaryExclusiveOr()
        {
            Expression left = MatchLogicalAndOrBinaryAnd();
            while (ScriptParser.tokenizer.Match('^'))
            {

                // Match ^= (assignment)
                if (ScriptParser.tokenizer.Match('='))
                {
                    if (!left.type.Equals(ExpressionType.Register))
                        throw new NslException("The left operand must be a variable", true);
                    left = new AssignmentExpression(left.integerValue, CreateMathematical(left, "^", MatchComplex()));
                    Scope.GetCurrent().AddVar(left.integerValue);
                }
                else

                // Matched ^
                {
                    left = CreateMathematical(left, "^", MatchLogicalAndOrBinaryAnd());
                }
            }

            return left;
        }

        private static Expression MatchLogicalOrOrBinaryInclusiveOr()
        {
            Expression left = MatchBinaryExclusiveOr();
            while (ScriptParser.tokenizer.Match('|'))
            {

                // Match ||
                if (ScriptParser.tokenizer.Match('|'))
                {
                    if (ExpressionType.IsBoolean(left) && left.booleanValue == true)
                    {

                        // Evaluated to true; we can ignore the rest of the expression.
                        while (ScriptParser.tokenizer.TokenNext("\")\" or \";\""))
                            if (ScriptParser.tokenizer.TokenIs(')') || ScriptParser.tokenizer.TokenIs(';'))
                                break;
                    }
                    else
                    {
                        left = CreateBoolean(left, "||", MatchBinaryExclusiveOr());
                    }
                }
                else 
// Match |= (assignment)
                if (ScriptParser.tokenizer.Match('='))
                {
                    if (!left.type.Equals(ExpressionType.Register))
                        throw new NslException("The left operand must be a variable", true);
                    left = new AssignmentExpression(left.integerValue, CreateMathematical(left, "|", MatchComplex()));
                    Scope.GetCurrent().AddVar(left.integerValue);
                }
                else

                // Matched |
                {
                    left = CreateMathematical(left, "|", MatchBinaryExclusiveOr());
                }
            }

            return left;
        }

        private static Expression MatchTernary()
        {
            Expression left = MatchLogicalOrOrBinaryInclusiveOr();
            while (ScriptParser.tokenizer.Match('?'))
            {

                // Matched ? :
                Expression ifTrue = MatchComplex();
                ScriptParser.tokenizer.MatchOrDie(':');
                left = CreateTernary(left, ifTrue, MatchLogicalOrOrBinaryInclusiveOr());
            }

            return left;
        }

        public static Expression MatchComplex()
        {
            Expression expression = MatchTernary();

            // Matched a register that isn't being assigned to. Check its scope.
            if (!(expression is AssembleExpression) && expression.type.Equals(ExpressionType.Register))
                Scope.GetCurrent().Check(expression.integerValue);
            return expression;
        }

        public static Expression MatchConstant(int returns)
        {

            // NSIS instruction?
            Expression value = Statement.MatchInstruction(returns);
            if (value != null)
                return value;
            string name = ScriptParser.tokenizer.MatchAWord("a function, constant or macro identifier");

            // returnvar(n)
            // Gets the return register #n being assigned to
            if (name.Equals("returnvar"))
            {
                List<Expression> paramsList = MatchList();
                if (paramsList.Count != 1)
                    throw new NslArgumentException("returnvar", 1);
                value = paramsList[0];
                if (!ExpressionType.IsInteger(value))
                    throw new NslArgumentException("returnvar", 1, ExpressionType.Integer);
                return new ReturnVarExpression(value.integerValue);
            }


            // toint(arg)
            // Converts a literal to an integer literal
            if (name.Equals("toint"))
            {
                List<Expression> paramsList = MatchList();
                int paramsCount = paramsList.Count;
                if (paramsCount < 1 || paramsCount > 2)
                    throw new NslArgumentException("toint", 1, 2);
                value = paramsList[0];
                if (!value.IsLiteral())
                    throw new NslArgumentException("toint", 1, true);
                Expression defaultValue;
                if (paramsCount > 1)
                {
                    defaultValue = paramsList[1];
                    if (!ExpressionType.IsInteger(defaultValue))
                        throw new NslArgumentException("toint", 2, ExpressionType.Integer);
                }
                else
                    defaultValue = null;

                // String literals are parsed as plain integers or as hexadecimal (0x...).
                if (value.type.Equals(ExpressionType.String) || value.type.Equals(ExpressionType.StringSpecial))
                {
                    string stringValue = value.ToString(true);
                    try
                    {
                        if (stringValue.StartsWith("0x"))
                            return Expression.FromInteger(Integer.ParseInt(stringValue, 16));
                        return Expression.FromInteger(Integer.ParseInt(stringValue));
                    }
                    catch (Exception ex)
                    {
                        if (defaultValue == null)
                            NslException.PrintWarning("\"toint\" could not convert string \"" + stringValue + "\" to an integer. " + ex.GetMessage() + ". Returned 0");
                    }

                    return defaultValue == null ? Expression.FromInteger(0) : defaultValue;
                }
                else 
// Boolean true == 1, false == 0.
                if (value.type.Equals(ExpressionType.Boolean))
                {
                    if (value.booleanValue == true)
                        return Expression.FromInteger(1);
                    return defaultValue == null ? Expression.FromInteger(0) : defaultValue;
                }
                else 
// No conversion needed for an integer literal.
                if (value.type.Equals(ExpressionType.Integer))
                {
                    return value;
                }
                else 
// For a register or NSIS constant, we return the internal index.
                if (value.type.Equals(ExpressionType.Register) || value.type.Equals(ExpressionType.Constant))
                {
                    return Expression.FromInteger(value.integerValue);
                }

                throw new NslException("\"intval\" cannot handle an input type of \"" + value.type + "\"", true);
            }


            // eval(string)
            // Evaluates the string argument
            if (name.Equals("eval"))
            {
                List<Expression> paramsList = MatchList();
                if (paramsList.Count != 1)
                    throw new NslArgumentException("eval", 1);
                value = paramsList[0];
                if (!ExpressionType.IsString(value))
                    throw new NslArgumentException("eval", 1, ExpressionType.String);
                ScriptParser.PushTokenizer(new Tokenizer(new StringReader(value.stringValue), "eval"));
                if (returns == 0)
                    return null;
                if (returns > 1)
                    return MatchConstant(returns);
                return MatchComplex();
            }


            // StrCmp(a, b)
            // Compare two string values for equality
            // Replaces the NSIS StrCmp instruction
            if (name.Equals("StrCmp"))
            {
                List<Expression> paramsList = MatchList();
                if (paramsList.Count != 2)
                    throw new NslArgumentException("StrCmp", 2);
                return CreateComparison(paramsList[0], "==", paramsList[1], ComparisonType.String);
            }


            // StrCmpS(a, b)
            // Compare two string values for equality, case sensitively
            // Replaces the NSIS StrCmpS instruction
            if (name.Equals("StrCmpS"))
            {
                List<Expression> paramsList = MatchList();
                if (paramsList.Count != 2)
                    throw new NslArgumentException("StrCmpS", 2);
                return CreateComparison(paramsList[0], "==", paramsList[1], ComparisonType.StringCaseSensitive);
            }


            // defined(const1, const2, ...)
            // true if all constants in the list are defined
            if (name.Equals("defined"))
            {
                ScriptParser.tokenizer.MatchOrDie('(');
                if (ScriptParser.tokenizer.Match(')'))
                    throw new NslException("\"defined\" expects one or more constant names", true);
                bool result = true;
                while (true)
                {
                    string constant = ScriptParser.tokenizer.MatchAWord("a constant name");
                    if (result && DefineList.Lookup(constant) == null)
                        result = false;
                    if (ScriptParser.tokenizer.Match(')'))
                        break;
                    ScriptParser.tokenizer.MatchOrDie(',');
                }

                return Expression.FromBoolean(result);
            }


            // type(expr)
            // the type of the given expression
            if (name.Equals("type"))
            {
                List<Expression> paramsList = MatchList();
                if (paramsList.Count != 1)
                    throw new NslArgumentException("type", 1);
                value = paramsList[0];
                if (value.type != null && !(value is AssembleExpression))
                {
                    switch (value.type)
                    {
                        case String:
                        case StringSpecial:
                            return Expression.FromString("String");
                        case Register:
                            return Expression.FromString("Register");
                        case Constant:
                            return Expression.FromString("Constant");
                        case Integer:
                            return Expression.FromString("Integer");
                        case Boolean:
                            return Expression.FromString("Boolean");
                    }
                }

                return Expression.FromString("Nonliteral");
            }


            // format("format_str", arg1, arg2, ...)
            // inserts the arguments into format_str in place of {0}, {1} etc.
            if (name.Equals("format"))
            {
                List<Expression> paramsList = MatchList();
                int paramsCount = paramsList.Count - 1;
                if (paramsCount < 1)
                    throw new NslArgumentException("format", 2, 999);
                value = paramsList[0];
                if (!ExpressionType.IsString(value))
                    throw new NslArgumentException("format", 1, ExpressionType.String);
                string formatString = value.ToString(true);
                int formatStringLength = formatString.Length();
                for (int i = 0; i < formatStringLength; i++)
                {

                    // Two { characters escapes.
                    if (formatString.CharAt(i) == '{' && formatString.CharAt(++i) != '{')
                    {
                        int paramNumberAt = i - 1;
                        string paramNumberString = "";
                        for (; i < formatStringLength; i++)
                        {
                            char c = formatString.CharAt(i);
                            if (c == '}')
                                break;
                            if (c < '0' || c > '9')
                                throw new NslException("Bad parameter number for \"format\" (contains non numeric characters)", true);
                            paramNumberString += c;
                        }

                        int paramNumber = Integer.ParseInt(paramNumberString);
                        if (paramNumber < 0 || paramNumber >= paramsCount)
                            throw new NslException("Parameter number for \"format\" is out of range of given parameters", true);

                        // Insert the parameter.
                        string paramValue = paramsList[paramNumber + 1].ToString(true);
                        formatString = formatString.Substring(0, paramNumberAt) + paramValue + formatString.Substring(i + 1);

                        // Move the new position to after the inserted parameter.
                        i = paramNumberAt + paramValue.Length();
                    }
                }

                return Expression.FromString(formatString);
            }


            // length(literal)
            // length of the given literal, e.g. length($R0) = 3, length(99) = 2
            if (name.Equals("length"))
            {
                List<Expression> paramsList = MatchList();
                if (paramsList.Count != 1)
                    throw new NslArgumentException("length", 1);
                value = paramsList[0];
                if (!value.IsLiteral())
                    throw new NslArgumentException("length", 1, true);
                return Expression.FromInteger(value.ToString(true).Length());
            }


            // nsisconst(name)
            // returns an NSIS constant of the given name, i.e. ${name}
            if (name.Equals("nsisconst"))
            {
                ScriptParser.tokenizer.MatchOrDie('(');
                string constant = ScriptParser.tokenizer.MatchAWord("a constant name");
                ScriptParser.tokenizer.MatchOrDie(')');
                return Expression.FromString("${" + constant + "}");
            }


            // Get a local macro constant (macro parameter) or a global constant.
            value = DefineList.Lookup(name);
            if (value != null)
            {

                // Late evaluation constant or a function or plug-in call follows.
                if (value.type.Equals(ExpressionType.StringSpecial) && value.booleanValue == true || ScriptParser.tokenizer.TokenIs('('))
                {
                    ScriptParser.PushTokenizer(new Tokenizer(new StringReader(value.ToString(true)), "constant \"" + name + "\""));
                    if (returns == 0)
                        return null;
                    if (returns > 1)
                        return MatchConstant(returns);
                    return MatchComplex();
                }

                return value;
            }


            // A plug-in call?
            if (ScriptParser.tokenizer.Match(':'))
            {
                ScriptParser.tokenizer.MatchOrDie(':');
                name = name + "::" + ScriptParser.tokenizer.MatchAWord("a plug-in function name");
                return new PluginCallExpression(name, Expression.MatchList());
            }


            // A function or macro call.
            if (ScriptParser.tokenizer.TokenIs('('))
            {
                List<Expression> paramsList = Expression.MatchList();
                List<Macro> macrosList = MacroList.GetCurrent()[name];
                if (!macrosList.IsEmpty())
                {
                    int paramsCount = paramsList.Count;

                    // Find a macro which matches the function call.
                    foreach (Macro macro in macrosList)
                    {
                        if (macro.GetParamCount() == paramsCount)
                        {
                            value = macro.Evaluate(paramsList, returns);

                            // Late evaluation constant was returned.
                            if (value.type.Equals(ExpressionType.StringSpecial) && value.booleanValue == true)
                            {
                                ScriptParser.PushTokenizer(new Tokenizer(new StringReader(value.stringValue), "macro \"" + name + "\""));
                                if (returns == 0)
                                    return null;
                                if (returns > 1)
                                    return MatchConstant(returns);
                                return MatchComplex();
                            }

                            return value;
                        }
                    }
                }


                // If it's not a macro insertion then it must be a function call.
                return new FunctionCallExpression(name, paramsList);
            }

            throw new NslException("Unrecognized constant or macro name \"" + name + "\"", true);
        }

        public static Expression Match()
        {
            if (ScriptParser.tokenizer.TokenIsWord() && !ScriptParser.tokenizer.sval.StartsWith("$") && !ScriptParser.tokenizer.sval.Equals("true") && !ScriptParser.tokenizer.sval.Equals("false"))
                return MatchConstant(1);
            return new Expression(ScriptParser.tokenizer);
        }

        public static Expression FindRegister(List<Expression> search, Expression find)
        {
            foreach (Expression var in search)
                if (var.type.Equals(ExpressionType.Register) && find.integerValue == var.integerValue)
                    return var;
            return null;
        }

        public static Expression FindRegister(List<Register> search, Register find)
        {
            foreach (Expression var in search)
                if (find.integerValue == var.integerValue)
                    return var;
            return null;
        }

        public static Expression FindRegister(HashMap<int, Expression> search, Expression find)
        {
            if (find.type.Equals(ExpressionType.Register))
                return search[Integer.ValueOf(find.integerValue)];
            return null;
        }

        public static Expression FindRegister(HashMap<int, Register> search, Register find)
        {
            return search[Integer.ValueOf(find.integerValue)];
        }

        public virtual string GetStringValue()
        {
            return this.stringValue;
        }

        public virtual int GetIntegerValue()
        {
            return this.integerValue;
        }

        public virtual bool GetBooleanValue()
        {
            return this.booleanValue;
        }

        public virtual ExpressionType GetType()
        {
            return this.type;
        }

        public virtual void SetInUse(bool inUse)
        {
        }

        public virtual string ToString()
        {
            return this.ToString(false);
        }

        public virtual string ToString(bool noQuote)
        {
            switch (this.type)
            {
                case String:
                case StringSpecial:
                    if (noQuote)
                        return this.stringValue;
                    return "\"" + this.stringValue + "\"";
                case Register:
                    return RegisterList.GetCurrent()[this.integerValue].ToString();
                case Constant:
                    return ConstantList.GetCurrent()[this.integerValue].ToString();
                case Integer:
                    return Integer.ToString(this.integerValue);
                case Boolean:
                    return Boolean.ToString(this.booleanValue);
            }

            return "?";
        }

        public static List<Expression> MatchList()
        {
            List<Expression> expressionList = new List<Expression>();
            ScriptParser.tokenizer.MatchOrDie('(');
            if (!ScriptParser.tokenizer.Match(')'))
            {
                while (true)
                {
                    expressionList.Add(MatchComplex());
                    if (ScriptParser.tokenizer.Match(')'))
                        break;
                    ScriptParser.tokenizer.MatchOrDie(',');
                }
            }

            return expressionList;
        }

        public static List<Expression> MatchRegisterList()
        {
            List<Expression> expressionList = new List<Expression>();
            ScriptParser.tokenizer.MatchOrDie('(');
            if (!ScriptParser.tokenizer.Match(')'))
            {
                while (true)
                {
                    Expression expression = Match();
                    if (!expression.type.Equals(ExpressionType.Register))
                        throw new NslExpectedException("Expected a register/variable");
                    expressionList.Add(expression);
                    if (ScriptParser.tokenizer.Match(')'))
                        break;
                    ScriptParser.tokenizer.MatchOrDie(',');
                }
            }

            return expressionList;
        }
    }
}