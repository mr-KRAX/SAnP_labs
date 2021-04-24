using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Lab5_Interpreter {
  class Program {
    static void Main(string[] args) {
      Console.WriteLine("Lab5 Interpreter: ");
      Expression textFixer = new TextFixer(new ManyTabs(),
                                           new SpareSpaces(), 
                                           new AppropriateQuotes(), 
                                           new HyphensAndDashes(), 
                                           new Linefeeds(),
                                           new SpontaneousReplacement());
      string rawText = "A lot of \t\t\ttabs! Many   ( unnecessary spare , spaces)   spaces! Wrong “quotes”! Hyphen - or dash—dash! IioO\r\n\r\n\r\n";
      Console.WriteLine("The text needs fixing: " + rawText);
      string fixedText = textFixer.Interpret(rawText);
      Console.WriteLine("Now it is much better: " + fixedText);
      string correctText = "A lot of\ttabs! Many (unnecessary spare, spaces) spaces! Wrong «quotes»! Hyphen — or dash-dash! !!00";
      Console.WriteLine("Correct text         : " + correctText);

    }
  }

  interface Expression {
    string Interpret(string context);
  }

  class TextFixer : Expression {
    private List<Expression> subExpressions;

    public TextFixer(params Expression[] exps) {
      subExpressions = new List<Expression>();
      foreach (var exp in exps)
        subExpressions.Add(exp);
    }
    public string Interpret(string context) {
      string interpreted = context;
      foreach (var expr in subExpressions)
        interpreted = expr.Interpret(interpreted);
      return interpreted;
    }
  }
  abstract class TerminalExpression : Expression {
    public abstract string Interpret(string context);
  }

  class AppropriateQuotes : TerminalExpression {
    public override string Interpret(String context) {
      string interpreted = context;
      interpreted = (new Regex("(“)+")).Replace(interpreted, "«");
      interpreted = (new Regex("(”)+")).Replace(interpreted, "»");
      return interpreted;
    }
  }

  class HyphensAndDashes : TerminalExpression {
    public override string Interpret(String context) {
      string interpreted = context;
      interpreted = (new Regex("( - )+")).Replace(interpreted, " — ");
      interpreted = (new Regex("( — )+")).Replace(interpreted, "$%#");
      interpreted = (new Regex("(—)+")).Replace(interpreted, "-");
      interpreted = (new Regex("(\\$%#)+")).Replace(interpreted, " — ");

      return interpreted;
    }
  }

  class ManyTabs : TerminalExpression {
    public override string Interpret(String context) {
      string interpreted = context;
      interpreted = (new Regex("\t+")).Replace(interpreted, "\t");
      interpreted = (new Regex("( \t)+")).Replace(interpreted, "\t");
      interpreted = (new Regex("(\t )+")).Replace(interpreted, "\t");
      interpreted = (new Regex("[ \t]+\n+")).Replace(interpreted, "\n");
      return interpreted;
    }
  }

  class Linefeeds : TerminalExpression {
    public override string Interpret(String context) {
      string interpreted = context;
      interpreted = (new Regex("(\r\n)+")).Replace(interpreted, "\r\n");
      return interpreted;
    }
  }

  class SpareSpaces : TerminalExpression {
    public override string Interpret(String context) {
      string interpreted = context;
      interpreted = (new Regex("^[ ]")).Replace(interpreted, "");
      interpreted = (new Regex("[ ]+")).Replace(interpreted, " ");
      interpreted = (new Regex("([(] )+")).Replace(interpreted, "(");
      interpreted = (new Regex("( [)])+")).Replace(interpreted, ")");
      interpreted = (new Regex("( ,)+")).Replace(interpreted, ",");
      interpreted = (new Regex("( [.])+")).Replace(interpreted, ".");
      interpreted = (new Regex("([“] )+")).Replace(interpreted, "“");
      interpreted = (new Regex("( [”])+")).Replace(interpreted, "”");
      interpreted = (new Regex("([«] )+")).Replace(interpreted, "«");
      interpreted = (new Regex("( [»])+")).Replace(interpreted, "»");
      interpreted = (new Regex("( [;])+")).Replace(interpreted, ";");
      return interpreted;
    }
  }

  class SpontaneousReplacement: TerminalExpression {
    public override string Interpret(string context) {
      string interpreted = context;
      interpreted = (new Regex("[Ii]")).Replace(interpreted, "!");
      interpreted = (new Regex("[oO]")).Replace(interpreted, "0");
      return interpreted;
    }
  }
}