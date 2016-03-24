/*
 * IfRebootFlagInstruction.java
 */

package nsl.instruction;

import java.io.IOException;
import java.util.ArrayList;
import java.util.EnumSet;
import nsl.*;
import nsl.expression.*;

/**
 * The NSIS IfRebootFlag instruction.
 * @author Stuart
 */
public class IfRebootFlagInstruction extends JumpExpression
{
  public static final String name = "RebootFlag";
  
  /**
   * Class constructor.
   * @param returns the number of return values
   */
  public IfRebootFlagInstruction(int returns)
  {
    if (!SectionInfo.in() && !FunctionInfo.in())
      throw new NslContextException(EnumSet.of(NslContext.Section, NslContext.Function), name);
    if (returns != 1)
      throw new NslReturnValueException(name, 1);

    ArrayList<Expression> paramsList = Expression.matchList();
    if (!paramsList.isEmpty())
      throw new NslArgumentException(name, 0);

    this.type = ExpressionType.Boolean;
    this.booleanValue = true;
  }
  
  /**
   * Assembles the source code.
   */
  @Override
  public void assemble() throws IOException
  {
    throw new UnsupportedOperationException("Not supported.");
  }

  /**
   * Assembles the source code.
   * @param var the variable to assign the value to
   */
  @Override
  public void assemble(Register var) throws IOException
  {
    if (this.thrownAwayAfterOptimise != null)
      AssembleExpression.assembleIfRequired(this.thrownAwayAfterOptimise);

    ScriptParser.writeLine("IfRebootFlag 0 +3");
    ScriptParser.writeLine("StrCpy " + var + " true");
    ScriptParser.writeLine("Goto +2");
    ScriptParser.writeLine("StrCpy " + var + " false");
  }

  /**
   * Assembles the source code.
   * @param gotoA the first go-to label
   * @param gotoB the second go-to label
   */
  @Override
  public void assemble(Label gotoA, Label gotoB) throws IOException
  {
    if (this.thrownAwayAfterOptimise != null)
      AssembleExpression.assembleIfRequired(this.thrownAwayAfterOptimise);

    if (this.booleanValue == true)
      ScriptParser.writeLine("IfRebootFlag " + gotoA + " " + gotoB);
    else
      ScriptParser.writeLine("IfRebootFlag " + gotoB + " " + gotoA);
  }
}
