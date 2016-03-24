/*
 * SwitchCaseStatement.java
 */

package nsl.statement;

import java.io.IOException;
import nsl.*;
import nsl.expression.*;

/**
 * Represents a case statement within a switch statement.
 * @author Stuart
 */
public class SwitchCaseStatement extends Statement
{
  private final Expression match;
  private Label label;

  /**
   * Class constructor.
   * @param label the label for the case
   */
  public SwitchCaseStatement()
  {
    ScriptParser.tokenizer.match("case");
    this.match = Expression.match();
    ScriptParser.tokenizer.matchOrDie(':');
  }

  /**
   * Sets the label for this case.
   * @param label the label for this case
   */
  public void setLabel(Label label)
  {
    this.label = label;
  }

  /**
   * Gets the label for this case.
   * @return the label for this case
   */
  public Label getLabel()
  {
    return this.label;
  }

  /**
   * Gets the basic expression that this case matches.
   * @return the basic expression that this case matches
   */
  public Expression getMatch()
  {
    return this.match;
  }

  /**
   * Assembles the source code.
   * @throws IOException
   */
  @Override
  public void assemble() throws IOException
  {
    this.label.write();
  }
}
