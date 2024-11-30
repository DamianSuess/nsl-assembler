/*
 * Statement.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Statement
{
    public abstract class Statement
    {
        protected static List<Statement> globalAssignmentStatements = new List<Statement>();
        protected static List<Statement> globalUninstallerAssignmentStatements = new List<Statement>();
        public static void AddGlobal(Statement add)
        {
            if (Scope.InUninstaller())
                globalUninstallerAssignmentStatements.Add(add);
            else
                globalAssignmentStatements.Add(add);
        }

        public static List<Statement> GetGlobal()
        {
            return globalAssignmentStatements;
        }

        public static List<Statement> GetGlobalUninstaller()
        {
            return globalUninstallerAssignmentStatements;
        }

        /// <summary>
        /// Abstract class constructor.
        /// </summary>
        protected Statement()
        {
        }

        public static AssembleExpression MatchInstruction(int returns)
        {
            if (ScriptParser.tokenizer.Match(AbortInstruction.name))
                return new AbortInstruction(returns);
            if (ScriptParser.tokenizer.Match(AddBrandingImageInstruction.name))
                return new AddBrandingImageInstruction(returns);
            if (ScriptParser.tokenizer.Match(AddSizeInstruction.name))
                return new AddSizeInstruction(returns);
            if (ScriptParser.tokenizer.Match(AllowRootDirInstallInstruction.name))
                return new AllowRootDirInstallInstruction(returns);
            if (ScriptParser.tokenizer.Match(AllowSkipFilesInstruction.name))
                return new AllowSkipFilesInstruction(returns);
            if (ScriptParser.tokenizer.Match(AutoCloseWindowInstruction.name))
                return new AutoCloseWindowInstruction(returns);
            if (ScriptParser.tokenizer.Match(BGFontInstruction.name))
                return new BGFontInstruction(returns);
            if (ScriptParser.tokenizer.Match(BGGradientInstruction.name))
                return new BGGradientInstruction(returns);
            if (ScriptParser.tokenizer.Match(BrandingTextInstruction.name))
                return new BrandingTextInstruction(returns);
            if (ScriptParser.tokenizer.Match(BringToFrontInstruction.name))
                return new BringToFrontInstruction(returns);
            if (ScriptParser.tokenizer.Match(CaptionInstruction.name))
                return new CaptionInstruction(returns);
            if (ScriptParser.tokenizer.Match(CheckBitmapInstruction.name))
                return new CheckBitmapInstruction(returns);
            if (ScriptParser.tokenizer.Match(ClearErrorsInstruction.name))
                return new ClearErrorsInstruction(returns);
            if (ScriptParser.tokenizer.Match(CompletedTextInstruction.name))
                return new CompletedTextInstruction(returns);
            if (ScriptParser.tokenizer.Match(ComponentTextInstruction.name))
                return new ComponentTextInstruction(returns);
            if (ScriptParser.tokenizer.Match(CopyFilesInstruction.name))
                return new CopyFilesInstruction(returns);
            if (ScriptParser.tokenizer.Match(CRCCheckInstruction.name))
                return new CRCCheckInstruction(returns);
            if (ScriptParser.tokenizer.Match(CreateDirectoryInstruction.name))
                return new CreateDirectoryInstruction(returns);
            if (ScriptParser.tokenizer.Match(CreateFontInstruction.name))
                return new CreateFontInstruction(returns);
            if (ScriptParser.tokenizer.Match(CreateShortCutInstruction.name) || ScriptParser.tokenizer.Match("CreateShortcut"))
                return new CreateShortCutInstruction(returns);
            if (ScriptParser.tokenizer.Match(DeleteInstruction.name))
                return new DeleteInstruction(returns);
            if (ScriptParser.tokenizer.Match(DeleteINISecInstruction.name))
                return new DeleteINISecInstruction(returns);
            if (ScriptParser.tokenizer.Match(DeleteINIStrInstruction.name))
                return new DeleteINIStrInstruction(returns);
            if (ScriptParser.tokenizer.Match(DeleteRegKeyInstruction.name))
                return new DeleteRegKeyInstruction(returns);
            if (ScriptParser.tokenizer.Match(DeleteRegValueInstruction.name))
                return new DeleteRegValueInstruction(returns);
            if (ScriptParser.tokenizer.Match(DetailPrintInstruction.name))
                return new DetailPrintInstruction(returns);
            if (ScriptParser.tokenizer.Match(DetailsButtonTextInstruction.name))
                return new DetailsButtonTextInstruction(returns);
            if (ScriptParser.tokenizer.Match(DirTextInstruction.name))
                return new DirTextInstruction(returns);
            if (ScriptParser.tokenizer.Match(DirVarInstruction.name))
                return new DirVarInstruction(returns);
            if (ScriptParser.tokenizer.Match(DirVerifyInstruction.name))
                return new DirVerifyInstruction(returns);
            if (ScriptParser.tokenizer.Match(EnableWindowInstruction.name))
                return new EnableWindowInstruction(returns);
            if (ScriptParser.tokenizer.Match(EnumRegKeyInstruction.name))
                return new EnumRegKeyInstruction(returns);
            if (ScriptParser.tokenizer.Match(EnumRegValueInstruction.name))
                return new EnumRegValueInstruction(returns);
            if (ScriptParser.tokenizer.Match(ExecInstruction.name))
                return new ExecInstruction(returns);
            if (ScriptParser.tokenizer.Match(ExecShellInstruction.name))
                return new ExecShellInstruction(returns);
            if (ScriptParser.tokenizer.Match(ExecWaitInstruction.name))
                return new ExecWaitInstruction(returns);
            if (ScriptParser.tokenizer.Match(ExpandEnvStringsInstruction.name))
                return new ExpandEnvStringsInstruction(returns);
            if (ScriptParser.tokenizer.Match(FileBufSizeInstruction.name))
                return new FileBufSizeInstruction(returns);
            if (ScriptParser.tokenizer.Match(FileCloseInstruction.name))
                return new FileCloseInstruction(returns);
            if (ScriptParser.tokenizer.Match(FileErrorTextInstruction.name))
                return new FileErrorTextInstruction(returns);
            if (ScriptParser.tokenizer.Match(FileInstruction.name))
                return new FileInstruction(returns);
            if (ScriptParser.tokenizer.Match(FileOpenInstruction.name))
                return new FileOpenInstruction(returns);
            if (ScriptParser.tokenizer.Match(FileReadByteInstruction.name))
                return new FileReadByteInstruction(returns);
            if (ScriptParser.tokenizer.Match(FileReadInstruction.name))
                return new FileReadInstruction(returns);
            if (ScriptParser.tokenizer.Match(FileRecursiveInstruction.name))
                return new FileRecursiveInstruction(returns);
            if (ScriptParser.tokenizer.Match(FileSeekInstruction.name))
                return new FileSeekInstruction(returns);
            if (ScriptParser.tokenizer.Match(FileWriteByteInstruction.name))
                return new FileWriteByteInstruction(returns);
            if (ScriptParser.tokenizer.Match(FileWriteInstruction.name))
                return new FileWriteInstruction(returns);
            if (ScriptParser.tokenizer.Match(FindCloseInstruction.name))
                return new FindCloseInstruction(returns);
            if (ScriptParser.tokenizer.Match(FindFirstInstruction.name))
                return new FindFirstInstruction(returns);
            if (ScriptParser.tokenizer.Match(FindNextInstruction.name))
                return new FindNextInstruction(returns);
            if (ScriptParser.tokenizer.Match(FindWindowInstruction.name))
                return new FindWindowInstruction(returns);
            if (ScriptParser.tokenizer.Match(FlushINIInstruction.name))
                return new FlushINIInstruction(returns);
            if (ScriptParser.tokenizer.Match(GetCurInstTypeInstruction.name))
                return new GetCurInstTypeInstruction(returns);
            if (ScriptParser.tokenizer.Match(GetDlgItemInstruction.name))
                return new GetDlgItemInstruction(returns);
            if (ScriptParser.tokenizer.Match(GetDLLVersionInstruction.name))
                return new GetDLLVersionInstruction(returns);
            if (ScriptParser.tokenizer.Match(GetDLLVersionLocalInstruction.name))
                return new GetDLLVersionLocalInstruction(returns);
            if (ScriptParser.tokenizer.Match(GetErrorLevelInstruction.name))
                return new GetErrorLevelInstruction(returns);
            if (ScriptParser.tokenizer.Match(GetFileTimeInstruction.name))
                return new GetFileTimeInstruction(returns);
            if (ScriptParser.tokenizer.Match(GetFileTimeLocalInstruction.name))
                return new GetFileTimeLocalInstruction(returns);
            if (ScriptParser.tokenizer.Match(GetInstDirErrorInstruction.name))
                return new GetInstDirErrorInstruction(returns);
            if (ScriptParser.tokenizer.Match(GetTempFileNameInstruction.name))
                return new GetTempFileNameInstruction(returns);
            if (ScriptParser.tokenizer.Match(HideWindowInstruction.name))
                return new HideWindowInstruction(returns);
            if (ScriptParser.tokenizer.Match(IconInstruction.name))
                return new IconInstruction(returns);
            if (ScriptParser.tokenizer.Match(IfAbortInstruction.name))
                return new IfAbortInstruction(returns);
            if (ScriptParser.tokenizer.Match(IfErrorsInstruction.name))
                return new IfErrorsInstruction(returns);
            if (ScriptParser.tokenizer.Match(IfFileExistsInstruction.name))
                return new IfFileExistsInstruction(returns);
            if (ScriptParser.tokenizer.Match(IfRebootFlagInstruction.name))
                return new IfRebootFlagInstruction(returns);
            if (ScriptParser.tokenizer.Match(IfSilentInstruction.name))
                return new IfSilentInstruction(returns);
            if (ScriptParser.tokenizer.Match(InitPluginsDirInstruction.name))
                return new InitPluginsDirInstruction(returns);
            if (ScriptParser.tokenizer.Match(InstProgressFlagsInstruction.name))
                return new InstProgressFlagsInstruction(returns);
            if (ScriptParser.tokenizer.Match(InstallDirInstruction.name))
                return new InstallDirInstruction(returns);
            if (ScriptParser.tokenizer.Match(InstallDirRegKeyInstruction.name))
                return new InstallDirRegKeyInstruction(returns);
            if (ScriptParser.tokenizer.Match(InstallButtonTextInstruction.name))
                return new InstallButtonTextInstruction(returns);
            if (ScriptParser.tokenizer.Match(InstallColorsInstruction.name))
                return new InstallColorsInstruction(returns);
            if (ScriptParser.tokenizer.Match(InstTypeGetTextInstruction.name))
                return new InstTypeGetTextInstruction(returns);
            if (ScriptParser.tokenizer.Match(InstTypeInstruction.name))
                return new InstTypeInstruction(returns);
            if (ScriptParser.tokenizer.Match(InstTypeSetTextInstruction.name))
                return new InstTypeSetTextInstruction(returns);
            if (ScriptParser.tokenizer.Match(IntFmtInstruction.name))
                return new IntFmtInstruction(returns);
            if (ScriptParser.tokenizer.Match(IsWindowInstruction.name))
                return new IsWindowInstruction(returns);
            if (ScriptParser.tokenizer.Match(LangStringInstruction.name))
                return new LangStringInstruction(returns);
            if (ScriptParser.tokenizer.Match(LoadLanguageFileInstruction.name))
                return new LoadLanguageFileInstruction(returns);
            if (ScriptParser.tokenizer.Match(LockWindowInstruction.name))
                return new LockWindowInstruction(returns);
            if (ScriptParser.tokenizer.Match(LogSetInstruction.name))
                return new LogSetInstruction(returns);
            if (ScriptParser.tokenizer.Match(LogTextInstruction.name))
                return new LogTextInstruction(returns);
            if (ScriptParser.tokenizer.Match(LicenseBkColorInstruction.name))
                return new LicenseBkColorInstruction(returns);
            if (ScriptParser.tokenizer.Match(LicenseDataInstruction.name))
                return new LicenseDataInstruction(returns);
            if (ScriptParser.tokenizer.Match(LicenseForceSelectionInstruction.name))
                return new LicenseForceSelectionInstruction(returns);
            if (ScriptParser.tokenizer.Match(LicenseLangStringInstruction.name))
                return new LicenseLangStringInstruction(returns);
            if (ScriptParser.tokenizer.Match(LicenseTextInstruction.name))
                return new LicenseTextInstruction(returns);
            if (ScriptParser.tokenizer.Match(MessageBoxInstruction.name))
                return new MessageBoxInstruction(returns);
            if (ScriptParser.tokenizer.Match(MiscButtonTextInstruction.name))
                return new MiscButtonTextInstruction(returns);
            if (ScriptParser.tokenizer.Match(NameInstruction.name))
                return new NameInstruction(returns);
            if (ScriptParser.tokenizer.Match(PopInstruction.name))
                return new PopInstruction(returns);
            if (ScriptParser.tokenizer.Match(PushInstruction.name))
                return new PushInstruction(returns);
            if (ScriptParser.tokenizer.Match(OutFileInstruction.name))
                return new OutFileInstruction(returns);
            if (ScriptParser.tokenizer.Match(QuitInstruction.name))
                return new QuitInstruction(returns);
            if (ScriptParser.tokenizer.Match(ReadEnvStrInstruction.name))
                return new ReadEnvStrInstruction(returns);
            if (ScriptParser.tokenizer.Match(ReadINIStrInstruction.name))
                return new ReadINIStrInstruction(returns);
            if (ScriptParser.tokenizer.Match(ReadRegDWORDInstruction.name))
                return new ReadRegDWORDInstruction(returns);
            if (ScriptParser.tokenizer.Match(ReadRegStrInstruction.name))
                return new ReadRegStrInstruction(returns);
            if (ScriptParser.tokenizer.Match(RebootInstruction.name))
                return new RebootInstruction(returns);
            if (ScriptParser.tokenizer.Match(RegDLLInstruction.name))
                return new RegDLLInstruction(returns);
            if (ScriptParser.tokenizer.Match(RenameInstruction.name))
                return new RenameInstruction(returns);
            if (ScriptParser.tokenizer.Match(RequestExecutionLevelInstruction.name))
                return new RequestExecutionLevelInstruction(returns);
            if (ScriptParser.tokenizer.Match(ReserveFileInstruction.name))
                return new ReserveFileInstruction(returns);
            if (ScriptParser.tokenizer.Match(ReserveFileRecursiveInstruction.name))
                return new ReserveFileRecursiveInstruction(returns);
            if (ScriptParser.tokenizer.Match(RMDirInstruction.name))
                return new RMDirInstruction(returns);
            if (ScriptParser.tokenizer.Match(RMDirRecursiveInstruction.name))
                return new RMDirRecursiveInstruction(returns);
            if (ScriptParser.tokenizer.Match(SearchPathInstruction.name))
                return new SearchPathInstruction(returns);
            if (ScriptParser.tokenizer.Match(SectionGetFlagsInstruction.name))
                return new SectionGetFlagsInstruction(returns);
            if (ScriptParser.tokenizer.Match(SectionGetInstTypesInstruction.name))
                return new SectionGetInstTypesInstruction(returns);
            if (ScriptParser.tokenizer.Match(SectionGetSizeInstruction.name))
                return new SectionGetSizeInstruction(returns);
            if (ScriptParser.tokenizer.Match(SectionGetTextInstruction.name))
                return new SectionGetTextInstruction(returns);
            if (ScriptParser.tokenizer.Match(SectionSetFlagsInstruction.name))
                return new SectionSetFlagsInstruction(returns);
            if (ScriptParser.tokenizer.Match(SectionSetInstTypesInstruction.name))
                return new SectionSetInstTypesInstruction(returns);
            if (ScriptParser.tokenizer.Match(SectionSetSizeInstruction.name))
                return new SectionSetSizeInstruction(returns);
            if (ScriptParser.tokenizer.Match(SectionSetTextInstruction.name))
                return new SectionSetTextInstruction(returns);
            if (ScriptParser.tokenizer.Match(SectionInInstruction.name))
                return new SectionInInstruction(returns);
            if (ScriptParser.tokenizer.Match(SendMessageInstruction.name))
                return new SendMessageInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetAutoCloseInstruction.name))
                return new SetAutoCloseInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetBrandingImageInstruction.name))
                return new SetBrandingImageInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetCompressInstruction.name))
                return new SetCompressInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetCompressorDictSizeInstruction.name))
                return new SetCompressorDictSizeInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetCompressorInstruction.name))
                return new SetCompressorInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetCtlColorsInstruction.name))
                return new SetCtlColorsInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetCurInstTypeInstruction.name))
                return new SetCurInstTypeInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetDatablockOptimizeInstruction.name))
                return new SetDatablockOptimizeInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetDateSaveInstruction.name))
                return new SetDateSaveInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetDetailsPrintInstruction.name))
                return new SetDetailsPrintInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetDetailsViewInstruction.name))
                return new SetDetailsViewInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetErrorLevelInstruction.name))
                return new SetErrorLevelInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetErrorsInstruction.name))
                return new SetErrorsInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetFileAttributesInstruction.name))
                return new SetFileAttributesInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetFontInstruction.name))
                return new SetFontInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetOverwriteInstruction.name))
                return new SetOverwriteInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetOutPathInstruction.name))
                return new SetOutPathInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetRebootFlagInstruction.name))
                return new SetRebootFlagInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetRegViewInstruction.name))
                return new SetRegViewInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetShellVarContextInstruction.name))
                return new SetShellVarContextInstruction(returns);
            if (ScriptParser.tokenizer.Match(SetSilentInstruction.name))
                return new SetSilentInstruction(returns);
            if (ScriptParser.tokenizer.Match(ShowInstDetailsInstruction.name))
                return new ShowInstDetailsInstruction(returns);
            if (ScriptParser.tokenizer.Match(ShowUninstDetailsInstruction.name))
                return new ShowUninstDetailsInstruction(returns);
            if (ScriptParser.tokenizer.Match(ShowWindowInstruction.name))
                return new ShowWindowInstruction(returns);
            if (ScriptParser.tokenizer.Match(SilentInstallInstruction.name))
                return new SilentInstallInstruction(returns);
            if (ScriptParser.tokenizer.Match(SilentUninstallInstruction.name))
                return new SilentUninstallInstruction(returns);
            if (ScriptParser.tokenizer.Match(SpaceTextsInstruction.name))
                return new SpaceTextsInstruction(returns);
            if (ScriptParser.tokenizer.Match(StrCpyInstruction.name))
                return new StrCpyInstruction(returns);
            if (ScriptParser.tokenizer.Match(StrLenInstruction.name))
                return new StrLenInstruction(returns);
            if (ScriptParser.tokenizer.Match(SubCaptionInstruction.name))
                return new SubCaptionInstruction(returns);
            if (ScriptParser.tokenizer.Match(UninstallButtonTextInstruction.name))
                return new UninstallIconInstruction(returns);
            if (ScriptParser.tokenizer.Match(UninstallCaptionInstruction.name))
                return new UninstallCaptionInstruction(returns);
            if (ScriptParser.tokenizer.Match(UninstallIconInstruction.name))
                return new UninstallButtonTextInstruction(returns);
            if (ScriptParser.tokenizer.Match(UninstallSubCaptionInstruction.name))
                return new UninstallSubCaptionInstruction(returns);
            if (ScriptParser.tokenizer.Match(UninstallTextInstruction.name))
                return new UninstallTextInstruction(returns);
            if (ScriptParser.tokenizer.Match(UnRegDLLInstruction.name))
                return new UnRegDLLInstruction(returns);
            if (ScriptParser.tokenizer.Match(VIAddVersionKeyInstruction.name))
                return new VIAddVersionKeyInstruction(returns);
            if (ScriptParser.tokenizer.Match(VIProductVersionInstruction.name))
                return new VIProductVersionInstruction(returns);
            if (ScriptParser.tokenizer.Match(WindowIconInstruction.name))
                return new WindowIconInstruction(returns);
            if (ScriptParser.tokenizer.Match(WriteINIStrInstruction.name))
                return new WriteINIStrInstruction(returns);
            if (ScriptParser.tokenizer.Match(WriteRegBinInstruction.name))
                return new WriteRegBinInstruction(returns);
            if (ScriptParser.tokenizer.Match(WriteRegDWORDInstruction.name))
                return new WriteRegDWORDInstruction(returns);
            if (ScriptParser.tokenizer.Match(WriteRegExpandStrInstruction.name))
                return new WriteRegExpandStrInstruction(returns);
            if (ScriptParser.tokenizer.Match(WriteRegStrInstruction.name))
                return new WriteRegStrInstruction(returns);
            if (ScriptParser.tokenizer.Match(WriteUninstallerInstruction.name))
                return new WriteUninstallerInstruction(returns);
            if (ScriptParser.tokenizer.Match(XPStyleInstruction.name))
                return new XPStyleInstruction(returns);
            return null;
        }

        public static Statement Match()
        {
            Statement statement = MatchInternal();

            // Matched assignment statements in the global scope need to be added to the
            // global statements list. Any function or plug-in calls in the global scope
            // (that aren't in an assignment statement) need to throw an error.
            if (statement != null && (statement is AssignmentStatement || statement is FunctionCallStatement) && !FunctionInfo.In() && !SectionInfo.In())
            {
                if (statement is FunctionCallStatement)
                {
                    if (((FunctionCallStatement)statement).GetExpression() is FunctionCallExpression)
                        throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), "function call");
                    if (((FunctionCallStatement)statement).GetExpression() is PluginCallExpression)
                        throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), "plug-in call");
                }


                // Add the statement to the global statements list and then match another
                // statement. If we are assigning "" to a variable then there is no need
                // to assemble anything. Registers are always initialized to "" in NSIS.
                if (!(statement is AssignmentStatement) || !ExpressionType.IsString(((AssignmentStatement)statement).GetExpression()) || !((AssignmentStatement)statement).GetExpression().GetStringValue().IsEmpty())
                    AddGlobal(statement);
                return Match();
            }

            return statement;
        }

        private static Statement MatchInternal()
        {
            if (ScriptParser.tokenizer.TokenIsWord())
            {

                // Exit if we're in an #if pre-processor directive and tokenizer hits
                // #else, #elseif or #endif.
                if (IfDirective.In())
                {
                    if (ScriptParser.tokenizer.TokenIs("#else") || ScriptParser.tokenizer.TokenIs("#elseif") || ScriptParser.tokenizer.TokenIs("#endif"))
                        return null;
                }


                // Assignment to a register. We could let Expression.matchConstant(0) do
                // this further down but that's a lot more unecessary string comparisons.
                // We know that this is a register by checking the first character and
                // therefore we must be assigning to it.
                if (ScriptParser.tokenizer.sval.StartsWith("$"))
                    return new AssignmentStatement();

                // Match a pre-processor directive.
                if (ScriptParser.tokenizer.Match("#define"))
                    return new DefineDirective();
                if (ScriptParser.tokenizer.Match("#redefine"))
                    return new RedefineDirective();
                if (ScriptParser.tokenizer.Match("#if"))
                    return new IfDirective();
                if (ScriptParser.tokenizer.Match("#macro"))
                    return new MacroDirective();
                if (ScriptParser.tokenizer.Match("#include"))
                    return new IncludeDirective();
                if (ScriptParser.tokenizer.Match("#undef"))
                    return new UndefDirective();
                if (ScriptParser.tokenizer.Match("#error"))
                    return new ErrorDirective();

                // Important that we don't use match() here otherwise it eats into the
                // NSIS code.
                if (ScriptParser.tokenizer.TokenIs("#nsis"))
                    return new NSISDirective();

                // #return directive is only valid in #macros.
                if (ScriptParser.tokenizer.Match("#return"))
                {
                    MacroDirective.MatchReturnDirective();
                    return Match();
                }


                // Prefixed with the uninstall key word.
                if (ScriptParser.tokenizer.Match("uninstall"))
                {
                    Statement statement = null;
                    Scope.SetInUninstaller(true);
                    if (ScriptParser.tokenizer.Match('{'))
                    {
                        statement = StatementList.Match();
                        ScriptParser.tokenizer.MatchOrDie('}');
                    }
                    else if (ScriptParser.tokenizer.Match("function"))
                        statement = new FunctionStatement();
                    else if (ScriptParser.tokenizer.Match("section"))
                        statement = new SectionStatement();
                    else if (ScriptParser.tokenizer.Match("sectiongroup"))
                        statement = new SectionGroupStatement();
                    else if (ScriptParser.tokenizer.Match("page"))
                        statement = new PageStatement();
                    if (statement == null)
                        throw new NslExpectedException("\"function\", \"section\", \"sectiongroup\" or \"page\"");
                    Scope.SetInUninstaller(false);
                    return statement;
                }

                if (ScriptParser.tokenizer.Match("function"))
                    return new FunctionStatement();
                if (ScriptParser.tokenizer.Match("section"))
                    return new SectionStatement();
                if (ScriptParser.tokenizer.Match("sectiongroup"))
                    return new SectionGroupStatement();
                if (ScriptParser.tokenizer.Match("page"))
                    return new PageStatement();
                if (ScriptParser.tokenizer.Match("if"))
                    return new IfStatement();
                if (ScriptParser.tokenizer.Match("while"))
                    return new WhileStatement();
                if (ScriptParser.tokenizer.Match("do"))
                    return new DoStatement();
                if (ScriptParser.tokenizer.Match("for"))
                    return new ForStatement();
                if (ScriptParser.tokenizer.Match("return"))
                    return new ReturnStatement();
                if (ScriptParser.tokenizer.Match("break"))
                    return new BreakStatement();
                if (ScriptParser.tokenizer.Match("continue"))
                    return new ContinueStatement();
                if (ScriptParser.tokenizer.Match("switch"))
                    return new SwitchStatement();

                // This will always be matched in an IfStatement, unless it's not in one!
                if (ScriptParser.tokenizer.Match("else"))
                    throw new NslException("\"else\" without matching \"if\" statement", true);

                // Match a constant. This can be an NSIS instruction, defined constant (to
                // evaluate the value of), function or plug-in call or a macro insertion.
                Expression expression = Expression.MatchConstant(0);

                // If null then the contents of a defined constant needs parsing.
                if (expression == null)
                    return Match();

                // If it can't be assembled, then it's no good for us!
                if (!(expression is AssembleExpression))
                    throw new NslException("\"" + expression.ToString(true) + "\" is not a valid statement", true);

                // Even macro calls need a ; on the end.
                ScriptParser.tokenizer.MatchEolOrDie();

                // Wrap the assignment expression within an assignment statement.
                if (expression is AssignmentExpression)
                    return new AssignmentStatement(expression);

                // Wrap the function or plug-in call inside a function call statement.
                if (expression is FunctionCallExpression || expression is PluginCallExpression)
                    return new FunctionCallStatement(expression);
                return new StatementExpression((AssembleExpression)expression);
            }

            if (ScriptParser.tokenizer.TokenIsChar())
            {

                // EOF found.
                if (ScriptParser.tokenizer.ttype == Tokenizer.TT_EOF)
                    return null;

                // Function call with multiple return values.
                if (ScriptParser.tokenizer.TokenIs('('))
                    return new FunctionCallStatement();

                // New block of code.
                if (ScriptParser.tokenizer.TokenIs('{'))
                    return new BlockStatement();

                // Skip ;
                if (ScriptParser.tokenizer.Match(';'))
                    return Match();

                // Odd character?
                if (!ScriptParser.tokenizer.TokenIs('}'))
                    throw new NslExpectedException("a statement");
            }

            return null;
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public abstract void Assemble();
    }
}