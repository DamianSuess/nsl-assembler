/*
 * MessageBoxInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;
using Nsl.Statement;

namespace Nsl.Instruction
{
    public class MessageBoxInstruction : JumpExpression
    {
        public static readonly string name = "MessageBox";
        private Expression options;
        private String[] optionsArray;
        private Expression body;
        private Expression silentButton;
        public MessageBoxInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 1)
                throw new NslReturnValueException(name, 0, 1);
            List<Expression> paramsList = Expression.MatchList();
            switch (paramsList.Count)
            {
                case 1:
                    this.optionsArray = new string[]
                    {
                        "MB_OK"
                    };
                    this.body = paramsList[0];
                    this.silentButton = null;
                    break;
                case 2:
                    this.options = paramsList[0];
                    if (!ExpressionType.IsString(this.options))
                        throw new NslArgumentException(name, 1, ExpressionType.String);
                    this.optionsArray = this.options.GetStringValue().Split("\\|");
                    this.body = paramsList[1];
                    this.silentButton = null;
                    break;
                case 3:
                    this.options = paramsList[0];
                    if (!ExpressionType.IsString(this.options))
                        throw new NslArgumentException(name, 1, ExpressionType.String);
                    this.optionsArray = this.options.GetStringValue().Split("\\|");
                    this.body = paramsList[1];
                    this.silentButton = paramsList[2];
                    if (!ExpressionType.IsString(this.silentButton))
                        throw new NslArgumentException(name, 3, ExpressionType.String);
                    break;
                default:
                    throw new NslArgumentException(name, 1, 3, paramsList.Count);
                    break;
            }

            foreach (string option in this.optionsArray)
            {
                if (!option.Equals("MB_OK") && !option.Equals("MB_OKCANCEL") && !option.Equals("MB_ABORTRETRYIGNORE") && !option.Equals("MB_RETRYCANCEL") && !option.Equals("MB_YESNO") && !option.Equals("MB_YESNOCANCEL") && !option.Equals("MB_ICONEXCLAMATION") && !option.Equals("MB_ICONINFORMATION") && !option.Equals("MB_ICONQUESTION") && !option.Equals("MB_ICONSTOP") && !option.Equals("MB_USERICON") && !option.Equals("MB_TOPMOST") && !option.Equals("MB_SETFOREGROUND") && !option.Equals("MB_RIGHT") && !option.Equals("MB_RTLREADING") && !option.Equals("MB_DEFBUTTON1") && !option.Equals("MB_DEFBUTTON2") && !option.Equals("MB_DEFBUTTON3") && !option.Equals("MB_DEFBUTTON4"))
                {
                    throw new NslArgumentException(name, 1, "\"" + option + "\"");
                }
            }

            if (this.silentButton != null)
            {
                string value = this.silentButton.GetStringValue();
                if (!IsValidReturnValue(value))
                {
                    throw new NslArgumentException(name, 3, "\"" + value + "\"");
                }
            }


            // The negate switch.
            this.booleanValue = false;

            // Not a Boolean type unless optimise gets called successfully.
            this.type = ExpressionType.Other;
        }

        // The negate switch.
        // Not a Boolean type unless optimise gets called successfully.
        private static bool IsValidReturnValue(string value)
        {
            return value.Equals("IDABORT") || value.Equals("IDCANCEL") || value.Equals("IDIGNORE") || value.Equals("IDNO") || value.Equals("IDOK") || value.Equals("IDRETRY") || value.Equals("IDYES");
        }

        // The negate switch.
        // Not a Boolean type unless optimise gets called successfully.
        private void WriteMessageBox(string append)
        {
            Expression varOrBody = AssembleExpression.GetRegisterOrExpression(this.body);
            this.WriteMessageBox(varOrBody.ToString(), append);
            varOrBody.SetInUse(false);
        }

        // The negate switch.
        // Not a Boolean type unless optimise gets called successfully.
        private void WriteMessageBox(string messageBody, string append)
        {
            string line = name + " ";
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
                    AssembleExpression.AssembleIfRequired(this.silentButton);
                line += " /SD " + this.silentButton.GetStringValue();
            }

            ScriptParser.WriteLine(line + append);
        }

        // The negate switch.
        // Not a Boolean type unless optimise gets called successfully.
        private string CreateAppendStr(Label gotoA, Label gotoB, String[] returnChecks)
        {
            string append = "";
            /*
     * At this point, this.stringValue is e.g. "IDYES" which is what we are
     * checking the result of the MessageBox should be.
     *
     * MessageBox takes the form of, e.g.:
     * MessageBox MB_YESNOCANCEL "text" IDYES gotoA IDNO gotoB
     *
     * Unfortunately NSIS only allows two Goto jumps on the end even if we have
     * 3 buttons. Therefore we would need to add Goto gotoB on the next line to
     * account for IDCANCEL.
     *
     * If gotoA or gotoB are relative jumps of 0 then we can omit that jump from
     * the end of the MessageBox.
     */

            // Can have a max of 2 jumps on a MessageBox but up to 3 buttons.
            if (returnChecks.length == 3)
            {

                // gotoA doesn't jump anywhere so find gotoB's return-check value and
                // ignore gotoA.
                if (gotoA.ToString().Equals("0"))
                {
                    foreach (string value in returnChecks)
                    {
                        if (!value.Equals(this.stringValue))
                        {
                            if (!gotoB.ToString().Equals("0"))
                                append += " " + value + " " + gotoB;
                        }
                    }
                }
                else 
// gotoB doesn't jump anywhere so find gotoA's return-check value and
                // ignore gotoB.
                if (gotoB.ToString().Equals("0"))
                {
                    foreach (string value in returnChecks)
                    {
                        if (value.Equals(this.stringValue))
                        {
                            if (!gotoA.ToString().Equals("0"))
                                append += " " + value + " " + gotoA;
                        }
                    }
                }
                else

                // We need to use gotoA and gotoB but we're stuck with 2 Goto jumps on the
                // end of our MessageBox (due to NSIS) so we have to stick a Goto instr.
                // after our MessageBox instr.
                {
                    foreach (string value in returnChecks)
                    {
                        if (value.Equals(this.stringValue))
                        {
                            if (!gotoA.ToString().Equals("0"))
                                append = " " + value + " " + gotoA;
                            break;
                        }
                    }


                    // We'll have to stick a Goto on the next line...
                    append += "\r\nGoto " + gotoB;
                }
            }
            else
            {
                foreach (string value in returnChecks)
                {
                    if (value.Equals(this.stringValue))
                    {
                        if (!gotoA.ToString().Equals("0"))
                            append += " " + value + " " + gotoA;
                    }
                    else
                    {
                        if (!gotoB.ToString().Equals("0"))
                            append += " " + value + " " + gotoB;
                    }
                }
            }

            return append;
        }

        // The negate switch.
        // Not a Boolean type unless optimise gets called successfully.
        /*
     * At this point, this.stringValue is e.g. "IDYES" which is what we are
     * checking the result of the MessageBox should be.
     *
     * MessageBox takes the form of, e.g.:
     * MessageBox MB_YESNOCANCEL "text" IDYES gotoA IDNO gotoB
     *
     * Unfortunately NSIS only allows two Goto jumps on the end even if we have
     * 3 buttons. Therefore we would need to add Goto gotoB on the next line to
     * account for IDCANCEL.
     *
     * If gotoA or gotoB are relative jumps of 0 then we can omit that jump from
     * the end of the MessageBox.
     */
        // Can have a max of 2 jumps on a MessageBox but up to 3 buttons.
        // gotoA doesn't jump anywhere so find gotoB's return-check value and
        // ignore gotoA.
        // gotoB doesn't jump anywhere so find gotoA's return-check value and
        // ignore gotoB.
        // We need to use gotoA and gotoB but we're stuck with 2 Goto jumps on the
        // end of our MessageBox (due to NSIS) so we have to stick a Goto instr.
        // after our MessageBox instr.
        // We'll have to stick a Goto on the next line...
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            if (this.options != null)
                AssembleExpression.AssembleIfRequired(this.options);
            if (this.thrownAwayAfterOptimise != null)
                AssembleExpression.AssembleIfRequired(this.thrownAwayAfterOptimise);
            this.WriteMessageBox("");
        }

        // The negate switch.
        // Not a Boolean type unless optimise gets called successfully.
        /*
     * At this point, this.stringValue is e.g. "IDYES" which is what we are
     * checking the result of the MessageBox should be.
     *
     * MessageBox takes the form of, e.g.:
     * MessageBox MB_YESNOCANCEL "text" IDYES gotoA IDNO gotoB
     *
     * Unfortunately NSIS only allows two Goto jumps on the end even if we have
     * 3 buttons. Therefore we would need to add Goto gotoB on the next line to
     * account for IDCANCEL.
     *
     * If gotoA or gotoB are relative jumps of 0 then we can omit that jump from
     * the end of the MessageBox.
     */
        // Can have a max of 2 jumps on a MessageBox but up to 3 buttons.
        // gotoA doesn't jump anywhere so find gotoB's return-check value and
        // ignore gotoA.
        // gotoB doesn't jump anywhere so find gotoA's return-check value and
        // ignore gotoB.
        // We need to use gotoA and gotoB but we're stuck with 2 Goto jumps on the
        // end of our MessageBox (due to NSIS) so we have to stick a Goto instr.
        // after our MessageBox instr.
        // We'll have to stick a Goto on the next line...
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble(Register var)
        {
            string bodyNew;
            if (this.options != null)
                AssembleExpression.AssembleIfRequired(this.options);
            if (this.thrownAwayAfterOptimise != null)
                AssembleExpression.AssembleIfRequired(this.thrownAwayAfterOptimise);
            if (this.body is AssembleExpression)
            {
                ((AssembleExpression)this.body).Assemble(var);
                bodyNew = var.ToString();
            }
            else
            {
                bodyNew = this.body.ToString();
            }

            Label gotoA = LabelList.GetCurrent().GetNext();
            Label gotoB = LabelList.GetCurrent().GetNext();

            // Switch labels around if ! operator was used.
            if (this.booleanValue)
            {
                Label gotoTemp = gotoA;
                gotoA = gotoB;
                gotoB = gotoTemp;
            }

            foreach (string option in this.optionsArray)
            {
                if (option.Equals("MB_OK"))
                {
                    if (this.stringValue == null)
                    {
                        this.WriteMessageBox(bodyNew, "");
                        ScriptParser.WriteLine("StrCpy " + var + " IDOK");
                    }
                    else
                    {
                        this.WriteMessageBox(bodyNew, "");
                        if (this.booleanValue)
                            ScriptParser.WriteLine("StrCpy " + var + " true");
                        else
                            ScriptParser.WriteLine("StrCpy " + var + " false");
                    }

                    break;
                }

                if (option.Equals("MB_OKCANCEL"))
                {
                    if (this.stringValue == null)
                    {
                        this.WriteMessageBox(bodyNew, " IDCANCEL +3");
                        ScriptParser.WriteLine("StrCpy " + var + " IDOK");
                        ScriptParser.WriteLine("Goto +2");
                        ScriptParser.WriteLine("StrCpy " + var + " IDCANCEL");
                    }
                    else
                    {
                        this.WriteMessageBox(bodyNew, CreateAppendStr(gotoA, gotoB, new string[] { "IDOK", "IDCANCEL" }));
                        gotoA.Write();
                        ScriptParser.WriteLine("StrCpy " + var + " true");
                        ScriptParser.WriteLine("Goto +2");
                        gotoB.Write();
                        ScriptParser.WriteLine("StrCpy " + var + " false");
                    }

                    break;
                }

                if (option.Equals("MB_ABORTRETRYIGNORE"))
                {
                    if (this.stringValue == null)
                    {
                        this.WriteMessageBox(bodyNew, " IDRETRY +3 IDIGNORE +5");
                        ScriptParser.WriteLine("StrCpy " + var + " IDABORT");
                        ScriptParser.WriteLine("Goto +4");
                        ScriptParser.WriteLine("StrCpy " + var + " IDRETRY");
                        ScriptParser.WriteLine("Goto +2");
                        ScriptParser.WriteLine("StrCpy " + var + " IDIGNORE");
                    }
                    else
                    {
                        this.WriteMessageBox(bodyNew, CreateAppendStr(gotoA, gotoB, new string[] { "IDABORT", "IDRETRY", "IDIGNORE" }));
                        gotoA.Write();
                        ScriptParser.WriteLine("StrCpy " + var + " true");
                        ScriptParser.WriteLine("Goto +2");
                        gotoB.Write();
                        ScriptParser.WriteLine("StrCpy " + var + " false");
                    }

                    break;
                }

                if (option.Equals("MB_RETRYCANCEL"))
                {
                    if (this.stringValue == null)
                    {
                        this.WriteMessageBox(bodyNew, " IDCANCEL +3");
                        ScriptParser.WriteLine("StrCpy " + var + " IDRETRY");
                        ScriptParser.WriteLine("Goto +2");
                        ScriptParser.WriteLine("StrCpy " + var + " IDCANCEL");
                    }
                    else
                    {
                        this.WriteMessageBox(bodyNew, CreateAppendStr(gotoA, gotoB, new string[] { "IDRETRY", "IDCANCEL" }));
                        gotoA.Write();
                        ScriptParser.WriteLine("StrCpy " + var + " true");
                        ScriptParser.WriteLine("Goto +2");
                        gotoB.Write();
                        ScriptParser.WriteLine("StrCpy " + var + " false");
                    }

                    break;
                }

                if (option.Equals("MB_YESNO"))
                {
                    if (this.stringValue == null)
                    {
                        this.WriteMessageBox(bodyNew, " IDNO +3");
                        ScriptParser.WriteLine("StrCpy " + var + " IDYES");
                        ScriptParser.WriteLine("Goto +2");
                        ScriptParser.WriteLine("StrCpy " + var + " IDNO");
                    }
                    else
                    {
                        this.WriteMessageBox(bodyNew, CreateAppendStr(gotoA, gotoB, new string[] { "IDYES", "IDNO" }));
                        gotoA.Write();
                        ScriptParser.WriteLine("StrCpy " + var + " true");
                        ScriptParser.WriteLine("Goto +2");
                        gotoB.Write();
                        ScriptParser.WriteLine("StrCpy " + var + " false");
                    }

                    break;
                }

                if (option.Equals("MB_YESNOCANCEL"))
                {
                    if (this.stringValue == null)
                    {
                        this.WriteMessageBox(bodyNew, " IDNO +3 IDCANCEL +5");
                        ScriptParser.WriteLine("StrCpy " + var + " IDYES");
                        ScriptParser.WriteLine("Goto +4");
                        ScriptParser.WriteLine("StrCpy " + var + " IDNO");
                        ScriptParser.WriteLine("Goto +2");
                        ScriptParser.WriteLine("StrCpy " + var + " IDCANCEL");
                    }
                    else
                    {
                        this.WriteMessageBox(bodyNew, CreateAppendStr(gotoA, gotoB, new string[] { "IDYES", "IDNO", "IDCANCEL" }));
                        gotoA.Write();
                        ScriptParser.WriteLine("StrCpy " + var + " true");
                        ScriptParser.WriteLine("Goto +2");
                        gotoB.Write();
                        ScriptParser.WriteLine("StrCpy " + var + " false");
                    }

                    break;
                }
            }
        }

        // The negate switch.
        // Not a Boolean type unless optimise gets called successfully.
        /*
     * At this point, this.stringValue is e.g. "IDYES" which is what we are
     * checking the result of the MessageBox should be.
     *
     * MessageBox takes the form of, e.g.:
     * MessageBox MB_YESNOCANCEL "text" IDYES gotoA IDNO gotoB
     *
     * Unfortunately NSIS only allows two Goto jumps on the end even if we have
     * 3 buttons. Therefore we would need to add Goto gotoB on the next line to
     * account for IDCANCEL.
     *
     * If gotoA or gotoB are relative jumps of 0 then we can omit that jump from
     * the end of the MessageBox.
     */
        // Can have a max of 2 jumps on a MessageBox but up to 3 buttons.
        // gotoA doesn't jump anywhere so find gotoB's return-check value and
        // ignore gotoA.
        // gotoB doesn't jump anywhere so find gotoA's return-check value and
        // ignore gotoB.
        // We need to use gotoA and gotoB but we're stuck with 2 Goto jumps on the
        // end of our MessageBox (due to NSIS) so we have to stick a Goto instr.
        // after our MessageBox instr.
        // We'll have to stick a Goto on the next line...
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        // Switch labels around if ! operator was used.
        public override void Assemble(Label gotoA, Label gotoB)
        {
            if (this.stringValue == null)
                throw new NslException("\"" + name + "\" used with no return value check");

            // Switch labels around if ! operator was used.
            if (this.booleanValue)
            {
                Label gotoTemp = gotoA;
                gotoA = gotoB;
                gotoB = gotoTemp;
            }

            if (this.options != null)
                AssembleExpression.AssembleIfRequired(this.options);
            if (this.thrownAwayAfterOptimise != null)
                AssembleExpression.AssembleIfRequired(this.thrownAwayAfterOptimise);
            string append = "";
            foreach (string option in this.optionsArray)
            {
                if (option.Equals("MB_OK"))
                {
                    append = CreateAppendStr(gotoA, gotoB, new string[] { "IDOK" });
                    break;
                }

                if (option.Equals("MB_OKCANCEL"))
                {
                    append = CreateAppendStr(gotoA, gotoB, new string[] { "IDOK", "IDCANCEL" });
                    break;
                }

                if (option.Equals("MB_ABORTRETRYIGNORE"))
                {
                    append = CreateAppendStr(gotoA, gotoB, new string[] { "IDABORT", "IDRETRY", "IDIGNORE" });
                    break;
                }

                if (option.Equals("MB_RETRYCANCEL"))
                {
                    append = CreateAppendStr(gotoA, gotoB, new string[] { "IDRETRY", "IDCANCEL" });
                    break;
                }

                if (option.Equals("MB_YESNO"))
                {
                    append = CreateAppendStr(gotoA, gotoB, new string[] { "IDYES", "IDNO" });
                    break;
                }

                if (option.Equals("MB_YESNOCANCEL"))
                {
                    append = CreateAppendStr(gotoA, gotoB, new string[] { "IDYES", "IDNO", "IDCANCEL" });
                    break;
                }
            }

            this.WriteMessageBox(append);
        }

        // The negate switch.
        // Not a Boolean type unless optimise gets called successfully.
        /*
     * At this point, this.stringValue is e.g. "IDYES" which is what we are
     * checking the result of the MessageBox should be.
     *
     * MessageBox takes the form of, e.g.:
     * MessageBox MB_YESNOCANCEL "text" IDYES gotoA IDNO gotoB
     *
     * Unfortunately NSIS only allows two Goto jumps on the end even if we have
     * 3 buttons. Therefore we would need to add Goto gotoB on the next line to
     * account for IDCANCEL.
     *
     * If gotoA or gotoB are relative jumps of 0 then we can omit that jump from
     * the end of the MessageBox.
     */
        // Can have a max of 2 jumps on a MessageBox but up to 3 buttons.
        // gotoA doesn't jump anywhere so find gotoB's return-check value and
        // ignore gotoA.
        // gotoB doesn't jump anywhere so find gotoA's return-check value and
        // ignore gotoB.
        // We need to use gotoA and gotoB but we're stuck with 2 Goto jumps on the
        // end of our MessageBox (due to NSIS) so we have to stick a Goto instr.
        // after our MessageBox instr.
        // We'll have to stick a Goto on the next line...
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        // Switch labels around if ! operator was used.
        // Switch labels around if ! operator was used.
        public virtual void Assemble(List<SwitchCaseStatement> switchCases)
        {
            if (this.stringValue == null)
            {
                if (this.options != null)
                    AssembleExpression.AssembleIfRequired(this.options);
                if (this.thrownAwayAfterOptimise != null)
                    AssembleExpression.AssembleIfRequired(this.thrownAwayAfterOptimise);
                string append = "";
                foreach (SwitchCaseStatement caseStatement in switchCases)
                {
                    append += " " + caseStatement.GetMatch().GetStringValue() + " " + caseStatement.GetLabel();
                }

                this.WriteMessageBox(append);
            }
            else
            {
                Label gotoA = null;
                Label gotoB = null;
                foreach (SwitchCaseStatement caseStatement in switchCases)
                {
                    if (caseStatement.GetMatch().GetBooleanValue() == true)
                    {
                        if (gotoA == null)
                            gotoA = caseStatement.GetLabel();
                    }
                    else
                    {
                        if (gotoB == null)
                            gotoB = caseStatement.GetLabel();
                    }
                }

                this.Assemble(gotoA, gotoB);
            }
        }

        // The negate switch.
        // Not a Boolean type unless optimise gets called successfully.
        /*
     * At this point, this.stringValue is e.g. "IDYES" which is what we are
     * checking the result of the MessageBox should be.
     *
     * MessageBox takes the form of, e.g.:
     * MessageBox MB_YESNOCANCEL "text" IDYES gotoA IDNO gotoB
     *
     * Unfortunately NSIS only allows two Goto jumps on the end even if we have
     * 3 buttons. Therefore we would need to add Goto gotoB on the next line to
     * account for IDCANCEL.
     *
     * If gotoA or gotoB are relative jumps of 0 then we can omit that jump from
     * the end of the MessageBox.
     */
        // Can have a max of 2 jumps on a MessageBox but up to 3 buttons.
        // gotoA doesn't jump anywhere so find gotoB's return-check value and
        // ignore gotoA.
        // gotoB doesn't jump anywhere so find gotoA's return-check value and
        // ignore gotoB.
        // We need to use gotoA and gotoB but we're stuck with 2 Goto jumps on the
        // end of our MessageBox (due to NSIS) so we have to stick a Goto instr.
        // after our MessageBox instr.
        // We'll have to stick a Goto on the next line...
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        // Switch labels around if ! operator was used.
        // Switch labels around if ! operator was used.
        public override void CheckSwitchCases(List<SwitchCaseStatement> switchCases, int switchLineNo)
        {
            if (this.stringValue == null && switchCases.Count > 2)
                throw new NslException("Too many \"case\" values for \"switch\" statement (2 allowed)", switchLineNo);
            foreach (SwitchCaseStatement caseStatement in switchCases)
            {

                // Unless optimise() was called then stringValue will be null.
                if (this.stringValue == null)
                {
                    if (!caseStatement.GetMatch().GetType().Equals(ExpressionType.String) && !caseStatement.GetMatch().GetType().Equals(ExpressionType.StringSpecial) || !IsValidReturnValue(caseStatement.GetMatch().GetStringValue()))
                        throw new NslException("Invalid \"case\" value of " + caseStatement.GetMatch(), caseStatement.GetLineNo());
                }
                else

                // optimise() was called and therefore we should check the switch case
                // values are Boolean (base implementation of checkSwitchCases does this).
                {
                    base.CheckSwitchCases(switchCases, switchLineNo);
                }
            }
        }

        // The negate switch.
        // Not a Boolean type unless optimise gets called successfully.
        /*
     * At this point, this.stringValue is e.g. "IDYES" which is what we are
     * checking the result of the MessageBox should be.
     *
     * MessageBox takes the form of, e.g.:
     * MessageBox MB_YESNOCANCEL "text" IDYES gotoA IDNO gotoB
     *
     * Unfortunately NSIS only allows two Goto jumps on the end even if we have
     * 3 buttons. Therefore we would need to add Goto gotoB on the next line to
     * account for IDCANCEL.
     *
     * If gotoA or gotoB are relative jumps of 0 then we can omit that jump from
     * the end of the MessageBox.
     */
        // Can have a max of 2 jumps on a MessageBox but up to 3 buttons.
        // gotoA doesn't jump anywhere so find gotoB's return-check value and
        // ignore gotoA.
        // gotoB doesn't jump anywhere so find gotoA's return-check value and
        // ignore gotoB.
        // We need to use gotoA and gotoB but we're stuck with 2 Goto jumps on the
        // end of our MessageBox (due to NSIS) so we have to stick a Goto instr.
        // after our MessageBox instr.
        // We'll have to stick a Goto on the next line...
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        // Switch labels around if ! operator was used.
        // Switch labels around if ! operator was used.
        // Unless optimise() was called then stringValue will be null.
        // optimise() was called and therefore we should check the switch case
        // values are Boolean (base implementation of checkSwitchCases does this).
        public override bool Optimise(Expression returnCheck, string @operator)
        {
            if (returnCheck.GetType().Equals(ExpressionType.String))
            {
                this.thrownAwayAfterOptimise = returnCheck;
                this.type = ExpressionType.Boolean;
                this.booleanValue = @operator.Equals("!=");
                this.stringValue = returnCheck.GetStringValue();
                return true;
            }

            return false;
        }
    }
}