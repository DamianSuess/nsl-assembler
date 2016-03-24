[![GNU General Public License](https://img.shields.io/badge/license-GPL%20v2-blue.svg?style=flat-square)](http://www.gnu.org/licenses/gpl-2.0.html)
[![Travis CI](https://img.shields.io/travis/NSIS-Dev/nsl-assembler.svg?style=flat-square)](https://travis-ci.org/NSIS-Dev/nsl-assembler)

# nsL Assembler

This installs the nsL Assembler to the NSIS directory and adds a Compile
nsL Script right click option to `.nsl` files. [NSIS][nsis] must be installed
prior to installing the nsL Assembler.

## Build

The only pre-requistes for building are [Java][java] and [Ant][ant].

1. Clone repository `git clone https://github.com/NSIS-Dev/nsl-assembler nsL`
2. Change directory `cd nsL`
3. Build JAR `ant jar`
 
## License

This work is licensed under the [The GNU General Public License, Version 2](LICENSE.md).

## Changelog

1.0.3 - 18th April 2011
* Fixed bug resulting in `func() == true`, `func() == false`, `func() != true`
  or `func() != false` not being assembled correctly.
* Removed unused `linenoprev()` function from Tokenizer class.

1.0.2 - 28th March 2011
* Global assignments of "" to registers are no longer assembled
  (registers are intialised to an empty string by NSIS).
* Fixed `<` and `>` operators using incorrect jump labels.
* Jump instructions when used in `switch` statements now receive similar
  optimisations to their use in `if` statements. For example, `MessageBox`
  can be used with cases "IDYES", "IDNO", "IDCANCEL" and so on. IfSilent
  can be used with cases `true` or `false`. The go-to labels for each case
  will be used directly on the jump instruction (i.e. `IfSilent
  label_case_true label_case_false`) as is also done with `if` statements.
* Switch cases must have literal string, Boolean or integer values.
* Added missing 'default:' case for `switch` statements.
* Fixed Boolean values or instructions not being accepted as operands
  for a Boolean operator.
* Fixed `MessageBox` being accepted as a Boolean value.
* Fixed `MessageBox` using 3 goto jumps on the end when only 2 are
  allowed.
* Fixed `If` statements accepting a non-boolean expression.
* Fixed Boolean logic for `||` operator with left and right operands as
  relative/equality comparisons.
* Added error on literal division by zero.
* More example scripts!

1.0.1 - 27th March 2011
* Added `@` prefix for strings to disable escape sequences (`\r` `\n` etc.)
  being parsed.

1.0.0 - 26th March 2011
* First public release.

[nsis]: http://nsis.sourceforge.net/
[java]: https://java.com/en/download/
[ant]: https://ant.apache.org/
