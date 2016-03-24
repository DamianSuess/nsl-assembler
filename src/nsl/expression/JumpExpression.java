/*
 * JumpExpression.java
 */

package nsl.expression;

import java.io.IOException;
import nsl.Label;
import nsl.Register;

/**
 * An NSIS instruction which performs jumps.
 * @author Stuart
 */
public abstract class JumpExpression extends ComparisonExpression
{
  protected Expression thrownAwayAfterOptimise;
  
  /**
   * Assembles the source code.
   */
  @Override
  public abstract void assemble() throws IOException;

  /**
   * Assembles the source code.
   * @param var the variable to assign the value to
   */
  @Override
  public abstract void assemble(Register var) throws IOException;

  /**
   * Assembles the source code.
   * @param gotoA the first go-to label
   * @param gotoB the second go-to label
   */
  @Override
  public abstract void assemble(Label gotoA, Label gotoB) throws IOException;

  /**
   * Attempts to optimise the jump expression.
   *
   * For example, Silent() == true becomes:
   *   IfSilent jump_a jump_b
   *
   * Without this type of optimisation, it would instead become:
   *   IfSilent 0 +3
   *   StrCpy $SomeTempVar true
   *   Goto +2
   *   StrCpy $SomeTempVar false
   *   StrCmp $SomeTempVar true jump_a jump_b
   *
   * We reduce 5 instructions into 1 and save a register!
   *
   * @param returnCheck the return check expression
   * @return <code>true</code> if the expression could be optimised
   */
  public boolean optimise(Expression returnCheck)
  {
    if (returnCheck.getType() == ExpressionType.Boolean)
    {
      this.thrownAwayAfterOptimise = returnCheck;
      this.booleanValue = returnCheck.getBooleanValue();
      return true;
    }
    return false;
  }
}
