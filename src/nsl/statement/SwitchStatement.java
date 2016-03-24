/*
 * SwitchStatement.java
 */

package nsl.statement;

import java.io.IOException;
import java.util.ArrayList;
import java.util.EnumSet;
import nsl.*;
import nsl.expression.*;

/**
 * Represents a switch statement.
 * @author Stuart
 */
public class SwitchStatement extends Statement
{
  private final Expression switchExpression;
  private final ArrayList<Statement> statementList;
  private final ArrayList<SwitchCaseStatement> casesList;

  /**
   * Class constructor.
   */
  public SwitchStatement()
  {
    if (!SectionInfo.in() && !FunctionInfo.in())
      throw new NslContextException(EnumSet.of(NslContext.Section, NslContext.Function), "switch");

    ScriptParser.tokenizer.matchOrDie('(');
    this.switchExpression = Expression.matchComplex();
    ScriptParser.tokenizer.matchOrDie(')');
    ScriptParser.tokenizer.matchOrDie('{');

    // Set non-null values so that the block statement can contain break statements.
    CodeInfo.getCurrent().setBreakLabel(RelativeJump.Zero);

    this.statementList = new ArrayList<Statement>();
    this.casesList = new ArrayList<SwitchCaseStatement>();

    // Get the statements including case statements.
    while (true)
    {
      Statement statement;
      if (ScriptParser.tokenizer.tokenIs("case"))
      {
        statement = new SwitchCaseStatement();
        this.casesList.add((SwitchCaseStatement)statement);
      }
      else
      {
        statement = Statement.match();
        if (statement == null)
          break;
      }

      this.statementList.add(statement);
    }

    // Check the last statement is a break statement.
    boolean noBreak = true;
    if (!this.statementList.isEmpty())
    {
      Statement last = this.statementList.get(this.statementList.size() - 1);
      if (last instanceof BlockStatement)
        last = ((BlockStatement)last).getLast();
      if (last instanceof BreakStatement)
        noBreak = false;
    }
    if (noBreak)
      throw new NslException("A \"switch\" statement must end with a \"break\" statement", true);

    CodeInfo.getCurrent().setBreakLabel(null);

    ScriptParser.tokenizer.matchOrDie('}');
  }

  /**
   * Assembles the source code.
   * @throws IOException
   */
  @Override
  public void assemble() throws IOException
  {
    // Do not assemble anything if there are no cases!
    if (this.casesList.isEmpty())
      return;

    // Give each case a label.
    for (SwitchCaseStatement statement : this.casesList)
      statement.setLabel(LabelList.getCurrent().getNext());

    Label gotoEnd = LabelList.getCurrent().getNext();
    Label gotoStart = LabelList.getCurrent().getNext();

    ScriptParser.writeLine("Goto " + gotoStart);
    
    Label parentBreak = CodeInfo.getCurrent().setBreakLabel(gotoEnd);

    for (Statement statement : this.statementList)
      statement.assemble();
    
    CodeInfo.getCurrent().setBreakLabel(parentBreak);

    gotoStart.write();

    Expression varOrSwitchExpression = AssembleExpression.getRegisterOrExpression(this.switchExpression);
    for (SwitchCaseStatement statement : this.casesList)
    {
      if (statement.getMatch().getType() == ExpressionType.Integer)
        ScriptParser.writeLine(String.format("IntCmp %s %s %s", varOrSwitchExpression, statement.getMatch(), statement.getLabel()));
      else
        ScriptParser.writeLine(String.format("StrCmp%s %s %s %s", statement.getMatch().getType() == ExpressionType.StringSpecial ? "S" : "", varOrSwitchExpression, statement.getMatch(), statement.getLabel()));
    }
    varOrSwitchExpression.setInUse(false);

    gotoEnd.write();
  }
}
