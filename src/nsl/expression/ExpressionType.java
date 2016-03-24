/*
 * ExpressionType.java
 */

package nsl.expression;

/**
 * Enumeration which defines basic expression types.
 * @author Stuart
 */
public enum ExpressionType
{
  String,
  StringSpecial,
  Integer,
  Boolean,
  Register,
  Constant;

  /**
   * Returns <code>true</code> if the given expression is a literal string
   * type.
   * @param expression the expression
   * @return <code>true</code> if the expression is a literal string
   */
  public static boolean isString(Expression expression)
  {
    return expression.isLiteral() && expression.type == ExpressionType.String || expression.type == ExpressionType.StringSpecial;
  }

  /**
   * Returns <code>true</code> if the given expression is a literal Boolean
   * type.
   * @param expression the expression
   * @return <code>true</code> if the expression is a literal Boolean
   */
  public static boolean isBoolean(Expression expression)
  {
    return expression.isLiteral() && expression.type == ExpressionType.Boolean;
  }

  /**
   * Returns <code>true</code> if the given expression is a literal variable
   * type.
   * @param expression the expression
   * @return <code>true</code> if the expression is a literal variable
   */
  public static boolean isRegister(Expression expression)
  {
    return expression.isLiteral() && expression.type == ExpressionType.Register;
  }

  /**
   * Returns <code>true</code> if the given expression is a literal integer
   * type.
   * @param expression the expression
   * @return <code>true</code> if the expression is a literal integer
   */
  public static boolean isInteger(Expression expression)
  {
    return expression.isLiteral() && expression.type == ExpressionType.Integer;
  }
  
  /**
   * Returns a string representation of the current object.
   * @return a string representation of the current object
   */
  @Override
  public String toString()
  {
    switch (this)
    {
      case String:
        return "a string";
      case StringSpecial:
        return "an unquoted string";
      case Integer:
        return "an integer";
      case Register:
        return "a variable";
    }
    return "??";
  }
}
