/*
 * MessageBoxInstruction.java
 */

package nsl.instruction;

import java.io.IOException;
import java.util.ArrayList;
import java.util.EnumSet;
import nsl.*;
import nsl.expression.*;

/**
 * The NSIS MessageBox instruction.
 * @author Stuart
 */
public class MessageBoxInstruction extends JumpExpression
{
  public static final String name = "MessageBox";
  private Expression options;
  private String[] optionsArray;
  private Expression body;
  private Expression silentButton;
  
  /**
   * Class constructor.
   * @param returns the number of values to return
   */
  public MessageBoxInstruction(int returns)
  {
    if (!SectionInfo.in() && !FunctionInfo.in())
      throw new NslContextException(EnumSet.of(NslContext.Section, NslContext.Function), name);
    if (returns > 1)
      throw new NslReturnValueException(name, 0, 1);

    ArrayList<Expression> paramsList = Expression.matchList();
    switch (paramsList.size())
    {
      case 1:
        this.optionsArray = new String[] { "MB_OK" };
        this.body = paramsList.get(0);
        this.silentButton = null;
        break;
      case 2:
        this.options = paramsList.get(0);
        if (!ExpressionType.isString(this.options))
          throw new NslArgumentException(name, 1, ExpressionType.String);
        this.optionsArray = this.options.toString(true).split("\\|");
        this.body = paramsList.get(1);
        this.silentButton = null;
        break;
      case 3:
        this.options = paramsList.get(0);
        if (!ExpressionType.isString(this.options))
          throw new NslArgumentException(name, 1, ExpressionType.String);
        this.optionsArray = this.options.toString(true).split("\\|");
        this.body = paramsList.get(1);
        this.silentButton = paramsList.get(2);
        if (!ExpressionType.isString(this.silentButton))
          throw new NslArgumentException(name, 3, ExpressionType.String);
        break;
      default:
        throw new NslArgumentException(name, 1, 3, paramsList.size());
    }

    for (String option : this.optionsArray)
    {
      if (!option.equals("MB_OK") &&
          !option.equals("MB_OKCANCEL") &&
          !option.equals("MB_ABORTRETRYIGNORE") &&
          !option.equals("MB_RETRYCANCEL") &&
          !option.equals("MB_YESNO") &&
          !option.equals("MB_YESNOCANCEL") &&
          !option.equals("MB_ICONEXCLAMATION") &&
          !option.equals("MB_ICONINFORMATION") &&
          !option.equals("MB_ICONQUESTION") &&
          !option.equals("MB_ICONSTOP") &&
          !option.equals("MB_USERICON") &&
          !option.equals("MB_TOPMOST") &&
          !option.equals("MB_SETFOREGROUND") &&
          !option.equals("MB_RIGHT") &&
          !option.equals("MB_RTLREADING") &&
          !option.equals("MB_DEFBUTTON1") &&
          !option.equals("MB_DEFBUTTON2") &&
          !option.equals("MB_DEFBUTTON3") &&
          !option.equals("MB_DEFBUTTON4"))
      {
        throw new NslArgumentException(name, 1, "\"" + option + "\"");
      }
    }

    if (this.silentButton != null)
    {
      String value = this.silentButton.toString(true);
      if (!isValidReturnValue(value))
      {
        throw new NslArgumentException(name, 3, "\"" + value + "\"");
      }
    }

    // The negate switch.
    this.booleanValue = false;
  }

  /**
   * Attempts to optimise the jump expression.
   * @param returnCheck the return check expression
   * @return <code>true</code> if the expression could be optimised
   */
  @Override
  public boolean optimise(Expression returnCheck)
  {
    if (returnCheck.getType() == ExpressionType.String)
    {
      this.thrownAwayAfterOptimise = returnCheck;
      this.stringValue = returnCheck.getStringValue();
      return true;
    }
    return false;
  }
  
  /**
   * Determines if the given message box return value (such as IDOK) is valid.
   * @param value the value to validate
   * @return <code>true</code> if the value is valid
   */
  private static boolean isValidReturnValue(String value)
  {
    return
      value.equals("IDABORT") ||
      value.equals("IDCANCEL") ||
      value.equals("IDIGNORE") ||
      value.equals("IDNO") ||
      value.equals("IDOK") ||
      value.equals("IDRETRY") ||
      value.equals("IDYES");
  }

  /**
   * Writes the MessageBox instruction.
   * @param append a string to append to the end
   */
  private void writeMessageBox(String append) throws IOException
  {
    Expression varOrBody = AssembleExpression.getRegisterOrExpression(this.body);
    this.writeMessageBox(varOrBody.toString(), append);
    varOrBody.setInUse(false);
  }

  /**
   * Writes the MessageBox instruction.
   * @param messageBody the message to display
   * @param append a string to append to the end
   */
  private void writeMessageBox(String messageBody, String append) throws IOException
  {
    String line = name + " ";
    for (int i = 0; i < this.optionsArray.length; i++)
    {
      if (i == this.optionsArray.length - 1)
        line += this.optionsArray[i];
      else
        line += this.optionsArray[i] + '|';
    }
    line += " " + messageBody;
    if (this.silentButton != null)
    {
      if (this.silentButton != null)
        AssembleExpression.assembleIfRequired(this.silentButton);
      line += " /SD " + this.silentButton.toString(true);
    }
    ScriptParser.writeLine(line + append);
  }

  /**
   * Assembles the source code.
   */
  @Override
  public void assemble() throws IOException
  {
    if (this.options != null)
      AssembleExpression.assembleIfRequired(this.options);
    if (this.thrownAwayAfterOptimise != null)
      AssembleExpression.assembleIfRequired(this.thrownAwayAfterOptimise);

    this.writeMessageBox("");
  }

  /**
   * Assembles the source code.
   * @param var the variable to assign the value to
   */
  @Override
  public void assemble(Register var) throws IOException
  {
    String bodyNew;

    if (this.options != null)
      AssembleExpression.assembleIfRequired(this.options);
    if (this.thrownAwayAfterOptimise != null)
      AssembleExpression.assembleIfRequired(this.thrownAwayAfterOptimise);

    if (this.body instanceof AssembleExpression)
    {
      ((AssembleExpression)this.body).assemble(var);
      bodyNew = var.toString();
    }
    else
    {
      bodyNew = this.body.toString();
    }

    for (String option : this.optionsArray)
    {
      if (option.equalsIgnoreCase("MB_OK"))
      {
        this.writeMessageBox(bodyNew, "");
        ScriptParser.writeLine("StrCpy " + var + " IDOK");
        break;
      }
      if (option.equalsIgnoreCase("MB_OKCANCEL"))
      {
        this.writeMessageBox(bodyNew, " IDCANCEL +3");
        ScriptParser.writeLine("StrCpy " + var + " IDOK");
        ScriptParser.writeLine("Goto +2");
        ScriptParser.writeLine("StrCpy " + var + " IDCANCEL");
        break;
      }
      if (option.equalsIgnoreCase("MB_ABORTRETRYIGNORE"))
      {
        this.writeMessageBox(bodyNew, " IDRETRY +3 IDIGNORE +5");
        ScriptParser.writeLine("StrCpy " + var + " IDABORT");
        ScriptParser.writeLine("Goto +4");
        ScriptParser.writeLine("StrCpy " + var + " IDRETRY");
        ScriptParser.writeLine("Goto +2");
        ScriptParser.writeLine("StrCpy " + var + " IDIGNORE");
        break;
      }
      if (option.equalsIgnoreCase("MB_RETRYCANCEL"))
      {
        this.writeMessageBox(bodyNew, " IDCANCEL +3");
        ScriptParser.writeLine("StrCpy " + var + " IDRETRY");
        ScriptParser.writeLine("Goto +2");
        ScriptParser.writeLine("StrCpy " + var + " IDCANCEL");
        break;
      }
      if (option.equalsIgnoreCase("MB_YESNO"))
      {
        this.writeMessageBox(bodyNew, " IDNO +3");
        ScriptParser.writeLine("StrCpy " + var + " IDYES");
        ScriptParser.writeLine("Goto +2");
        ScriptParser.writeLine("StrCpy " + var + " IDNO");
        break;
      }
      if (option.equalsIgnoreCase("MB_YESNOCANCEL"))
      {
        this.writeMessageBox(bodyNew, " IDNO +3 IDCANCEL +5");
        ScriptParser.writeLine("StrCpy " + var + " IDYES");
        ScriptParser.writeLine("Goto +4");
        ScriptParser.writeLine("StrCpy " + var + " IDNO");
        ScriptParser.writeLine("Goto +2");
        ScriptParser.writeLine("StrCpy " + var + " IDCANCEL");
        break;
      }
    }
  }

  /**
   * Creates the string to append to the end of the message box instruction.
   * @param gotoA the first go-to label
   * @param gotoB the second go-to label
   * @param returnChecks the return check values (such as IDOK)
   * @return the append string
   */
  private String createAppendStr(Label gotoA, Label gotoB, String[] returnChecks)
  {
    String append = "";
    for (String value : returnChecks)
    {
      if (value.equals(this.stringValue))
        append += " " + value + " " + gotoA;
      else
        append += " " + value + " " + gotoB;
    }
    return append;
  }

  /**
   * Assembles the source code.
   * @param gotoA the first go-to label
   * @param gotoB the second go-to label
   */
  @Override
  public void assemble(Label gotoA, Label gotoB) throws IOException
  {
    if (this.stringValue == null)
      throw new NslException("\"" + name + "\" used with no return value check", true);

    if (this.booleanValue)
    {
      Label gotoTemp = gotoA;
      gotoA = gotoB;
      gotoB = gotoTemp;
    }
    
    if (this.options != null)
      AssembleExpression.assembleIfRequired(this.options);
    if (this.thrownAwayAfterOptimise != null)
      AssembleExpression.assembleIfRequired(this.thrownAwayAfterOptimise);

    String append = "";
    for (String option : this.optionsArray)
    {
      if (option.equalsIgnoreCase("MB_OK"))
      {
        append = createAppendStr(gotoA, gotoB, new String[] { "IDOK" });
        break;
      }
      if (option.equalsIgnoreCase("MB_OKCANCEL"))
      {
        append = createAppendStr(gotoA, gotoB, new String[] { "IDOK", "IDCANCEL" });
        break;
      }
      if (option.equalsIgnoreCase("MB_ABORTRETRYIGNORE"))
      {
        append = createAppendStr(gotoA, gotoB, new String[] { "IDABORT", "IDRETRY", "IDIGNORE" });
        break;
      }
      if (option.equalsIgnoreCase("MB_RETRYCANCEL"))
      {
        append = createAppendStr(gotoA, gotoB, new String[] { "IDRETRY", "IDCANCEL" });
        break;
      }
      if (option.equalsIgnoreCase("MB_YESNO"))
      {
        append = createAppendStr(gotoA, gotoB, new String[] { "IDYES", "IDNO" });
        break;
      }
      if (option.equalsIgnoreCase("MB_YESNOCANCEL"))
      {
        append = createAppendStr(gotoA, gotoB, new String[] { "IDYES", "IDNO", "IDCANCEL" });
        break;
      }
    }

    this.writeMessageBox(append);
  }
}
