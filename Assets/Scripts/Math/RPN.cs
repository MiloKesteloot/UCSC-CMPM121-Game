using System;
using System.Collections.Generic;

public class RPN
{
    public static float EvaluateExpression(string expression, Dictionary<string, float> variables = null) {
        variables ??= new Dictionary<string, float>();
        string[] tokens = expression.Split(" ");
        Stack<string> stack = new();
        List<string> operators = new(){"+", "-", "*", "/", "%", "**"};
        foreach (string token in tokens) {
            if (float.TryParse(token, out _)) {
                // Number
                stack.Push(token);
            } else if (variables.ContainsKey(token)) {
                // Variable
                stack.Push(variables[token].ToString());
            } else if (operators.Contains(token)) {
                // Operator
                float.TryParse(stack.Pop(), out float num2);
                float.TryParse(stack.Pop(), out float num1);
                switch (token) {
                    case "+":
                        stack.Push((num1 + num2).ToString());
                        break;
                    case "-":
                        stack.Push((num1 - num2).ToString());
                        break;
                    case "*":
                        stack.Push((num1 * num2).ToString());
                        break;
                    case "/":
                        stack.Push((num1 / num2).ToString());
                        break;
                    case "%":
                        stack.Push((num1 % num2).ToString());
                        break;
                    case "**":
                        stack.Push(Math.Pow(num1, num2).ToString());
                        break;
                }
            } else {
                // Invalid
                throw new ArithmeticException($"Invalid token '{token}' in expression '{expression}'.");
            }
        }
        float.TryParse(stack.Pop(), out float result);
        return result;
    }
}
