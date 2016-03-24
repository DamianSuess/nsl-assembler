/*
 * ExpressionTest.java
 */

package nsl.expression;

import nsl.Tokenizer;
import nsl.ScriptParser;
import java.io.StringReader;
import java.io.OutputStreamWriter;
import java.util.ArrayList;
import org.junit.After;
import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;
import static org.junit.Assert.*;

/**
 * Test the {@link nsl.expression.Expression} class.
 * @author Stuart
 */
public class ExpressionTest
{
  private static OutputStreamWriter outputStream;

  public ExpressionTest()
  {
  }

  @BeforeClass
  public static void setUpClass() throws Exception
  {
    outputStream = new OutputStreamWriter(System.out);
  }

  @AfterClass
  public static void tearDownClass() throws Exception
  {
    outputStream.close();
  }

  @Before
  public void setUp()
  {
  }

  @After
  public void tearDown()
  {
  }

  /**
   * Test of isLiteral method, of class Expression.
   */
  @Test
  public void testIsLiteral()
  {
    boolean ja = 9 == 8 + 5;
    System.out.println("isLiteral");
    ScriptParser.pushTokenizer(new Tokenizer(new StringReader("1 'hello' $var = 9 == (8 + 5) false true blah(99, 100)"), "ExpressionTest"));
    Expression e;
    System.out.println("  " + (e = Expression.matchComplex()));
    assertEquals(true, e.isLiteral());
    System.out.println("  " + (e = Expression.matchComplex()));
    assertEquals(true, e.isLiteral());
    System.out.println("  " + (e = Expression.matchComplex()));
    assertEquals(false, e.isLiteral());
    System.out.println("  " + (e = Expression.matchComplex()));
    assertEquals(true, e.isLiteral());
    System.out.println("  " + (e = Expression.matchComplex()));
    assertEquals(true, e.isLiteral());
    System.out.println("  " + (e = Expression.matchComplex()));
    assertEquals(false, e.isLiteral());
  }

  /**
   * Test of matchComplex method, of class Expression.
   */
  @Test
  public void testMatchComplex()
  {
    System.out.println("matchComplex");
    ScriptParser.pushTokenizer(new Tokenizer(new StringReader(
        "$var1 = $var2 = 3;\n"
      + "$var1 = 0;\n"
      + "$var2 = $var1 + $var1 + ($var1++) + 5 * $var1 - 3;\n"
      + "$var2 = $var1 == 5 && $var2 == 3 || $var2 == 9 || ($var1 = 9) == 3;\n"
      + "$var2 = $var1 <= 9 || $var2 <= 9 || $var2 >= 9 || $var2 > 9 && $var2 < 1 || $var1 == 9;\n"
      + "$var2 = $var1 < 5 || $var1++ < 3 || $var2-- != 3 && $var1++ >= 5;\n"
      + "$var2 = ($var1 | 5) == 34 || ($var2 | 3) == 99 || ($var2 & 2) == 2 && (($var3 = 3) ^ 3) == 5;\n"
      + "$var2 = 99 + ($var2 ^= $var2 -= $var1 << 9);\n"
      + "5 - (5 + 9) / 3 * (2 - 5) ^ 2 + 5 | (3 & 9);\n"
      + "44 * 3 / 5 + 9 + 3 + 9 - 2;\n"
      + "11 % 5 * 3 + 9 / ~4 - 3 - 4 + 2;\n"
      + "9 << 2 >> 1 + (3 << 2) >> 1;\n"
      + "true || false || true && false;\n"
      + "true == false || false != true || true == false && false != true;\n"
      ), "ExpressionTest"));

    String stringValue;
    System.out.println("  " + (stringValue = Expression.matchComplex().toString()));
    assertEquals("($var1 = ($var2 = 3))", stringValue);
    ScriptParser.tokenizer.matchEolOrDie();
    System.out.println("  " + (stringValue = Expression.matchComplex().toString()));
    assertEquals("($var1 = 0)", stringValue);
    ScriptParser.tokenizer.matchEolOrDie();
    System.out.println("  " + (stringValue = Expression.matchComplex().toString()));
    assertEquals("($var2 = (((($var1 + $var1) + ($var1 = ($var1 + 1))) + (5 * $var1)) - 3))", stringValue);
    ScriptParser.tokenizer.matchEolOrDie();
    System.out.println("  " + (stringValue = Expression.matchComplex().toString()));
    assertEquals("($var2 = (((($var1 == 5) && ($var2 == 3)) || ($var2 == 9)) || (($var1 = 9) == 3)))", stringValue);
    ScriptParser.tokenizer.matchEolOrDie();
    System.out.println("  " + (stringValue = Expression.matchComplex().toString()));
    assertEquals("($var2 = ((((($var1 <= 9) || ($var2 <= 9)) || ($var2 >= 9)) || (($var2 > 9) && ($var2 < 1))) || ($var1 == 9)))", stringValue);
    ScriptParser.tokenizer.matchEolOrDie();
    System.out.println("  " + (stringValue = Expression.matchComplex().toString()));
    assertEquals("($var2 = ((($var1 < 5) || (($var1 = ($var1 + 1)) < 3)) || ((($var2 = ($var2 - 1)) != 3) && (($var1 = ($var1 + 1)) >= 5))))", stringValue);
    ScriptParser.tokenizer.matchEolOrDie();
    System.out.println("  " + (stringValue = Expression.matchComplex().toString()));
    assertEquals("($var2 = (((($var1 | 5) == 34) || (($var2 | 3) == 99)) || ((($var2 & 2) == 2) && ((($var3 = 3) ^ 3) == 5))))", stringValue);
    ScriptParser.tokenizer.matchEolOrDie();
    System.out.println("  " + (stringValue = Expression.matchComplex().toString()));
    assertEquals("($var2 = (99 + ($var2 = ($var2 ^ ($var2 = ($var2 - ($var1 << 9)))))))", stringValue);
    ScriptParser.tokenizer.matchEolOrDie();

    int integerValue;
    Expression e;
    System.out.println("  " + (integerValue = Expression.matchComplex().getIntegerValue()));
    assertEquals(5 - (5 + 9) / 3 * (2 - 5) ^ 2 + 5 | (3 & 9), integerValue);
    ScriptParser.tokenizer.matchEolOrDie();
    System.out.println("  " + (integerValue = Expression.matchComplex().getIntegerValue()));
    assertEquals(44 * 3 / 5 + 9 + 3 + 9 - 2, integerValue);
    ScriptParser.tokenizer.matchEolOrDie();
    System.out.println("  " + (integerValue = Expression.matchComplex().getIntegerValue()));
    assertEquals(11 % 5 * 3 + 9 / ~4 - 3 - 4 + 2, integerValue);
    ScriptParser.tokenizer.matchEolOrDie();
    System.out.println("  " + (integerValue = Expression.matchComplex().getIntegerValue()));
    assertEquals(9 << 2 >> 1 + (3 << 2) >> 1, integerValue);
    ScriptParser.tokenizer.matchEolOrDie();

    boolean booleanValue;
    System.out.println("  " + (booleanValue = Expression.matchComplex().getBooleanValue()));
    assertEquals(true || false || true && false, booleanValue);
    ScriptParser.tokenizer.matchEolOrDie();
    System.out.println("  " + (booleanValue = Expression.matchComplex().getBooleanValue()));
    assertEquals(true == false || false != true || true == false && false != true, booleanValue);
    ScriptParser.tokenizer.matchEolOrDie();
  }

  /**
   * Test of matchConstant method, of class Expression.
   */
  @Test
  public void testMatchConstant()
  {
    System.out.println("matchConstant");
    int returns = 0;
    Expression expResult = null;
    Expression result = Expression.matchConstant(returns);
    assertEquals(expResult, result);
    // TODO review the generated test code and remove the default call to fail.
    fail("The test case is a prototype.");
  }

  /**
   * Test of match method, of class Expression.
   */
  @Test
  public void testMatch()
  {
    System.out.println("match");
    Expression expResult = null;
    Expression result = Expression.match();
    assertEquals(expResult, result);
    // TODO review the generated test code and remove the default call to fail.
    fail("The test case is a prototype.");
  }

  /**
   * Test of findRegister method, of class Expression.
   */
  @Test
  public void testFindRegister()
  {
    System.out.println("findVariable");
    ArrayList<Expression> search = null;
    Expression find = null;
    Expression expResult = null;
    Expression result = Expression.findRegister(search, find);
    assertEquals(expResult, result);
    // TODO review the generated test code and remove the default call to fail.
    fail("The test case is a prototype.");
  }

  /**
   * Test of getStringValue method, of class Expression.
   */
  @Test
  public void testGetStringValue()
  {
    System.out.println("getStringValue");
    Expression instance = new Expression();
    String expResult = "";
    String result = instance.getStringValue();
    assertEquals(expResult, result);
    // TODO review the generated test code and remove the default call to fail.
    fail("The test case is a prototype.");
  }

  /**
   * Test of getIntegerValue method, of class Expression.
   */
  @Test
  public void testGetIntegerValue()
  {
    System.out.println("getIntegerValue");
    Expression instance = new Expression();
    int expResult = 0;
    int result = instance.getIntegerValue();
    assertEquals(expResult, result);
    // TODO review the generated test code and remove the default call to fail.
    fail("The test case is a prototype.");
  }

  /**
   * Test of getBooleanValue method, of class Expression.
   */
  @Test
  public void testGetBooleanValue()
  {
    System.out.println("getBooleanValue");
    Expression instance = new Expression();
    boolean expResult = false;
    boolean result = instance.getBooleanValue();
    assertEquals(expResult, result);
    // TODO review the generated test code and remove the default call to fail.
    fail("The test case is a prototype.");
  }

  /**
   * Test of getType method, of class Expression.
   */
  @Test
  public void testGetType()
  {
    System.out.println("getType");
    Expression instance = new Expression();
    ExpressionType expResult = null;
    ExpressionType result = instance.getType();
    assertEquals(expResult, result);
    // TODO review the generated test code and remove the default call to fail.
    fail("The test case is a prototype.");
  }

  /**
   * Test of toString method, of class Expression.
   */
  @Test
  public void testToString()
  {
    System.out.println("toString");
    Expression instance = new Expression();
    String expResult = "";
    String result = instance.toString();
    assertEquals(expResult, result);
    // TODO review the generated test code and remove the default call to fail.
    fail("The test case is a prototype.");
  }

  /**
   * Test of matchList method, of class Expression.
   */
  @Test
  public void testMatchList()
  {
    System.out.println("matchList");
    ArrayList expResult = null;
    ArrayList result = Expression.matchList();
    assertEquals(expResult, result);
    // TODO review the generated test code and remove the default call to fail.
    fail("The test case is a prototype.");
  }

}