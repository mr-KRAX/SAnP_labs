using System;
using System.Collections.Generic;

namespace Lab6_Observer {
  class Program {
    static void Main(string[] args) {

      DeansOffice office = new DeansOffice();

      Department math = new Department("Of Applied Math", office);
      Department sciene = new Department("Of Applied Science", office);
      Console.WriteLine("- All deps are ready. Notifying deps about the deadline...");
      office.NotifyAboutDeadline();
      Console.WriteLine("\n- Sending reports...");
      math.SendReport();
      Console.WriteLine("\n- Checking missing reports...");
      office.CheckReports();
    }
  }

  public interface IObserver {
    void Update(string msg);
  }

  public interface IObservable {
    void Register(string name, IObserver o);
    void Remove(IObserver o);
    void NotifyAll(string msg);
  }

  public class Department : IObserver {
    public string name { get; private set; }
    private IObservable office;

    public Department(string name, IObservable office) {
      this.name = name;
      this.office = office;
      office.Register(name, this);
    }

    public void Update(string msg) {
      Console.WriteLine($"Dep. \"{name}\" gets message: {msg}");
    }

    public void SendReport() {
      Random rand = new Random();
      float score = ((float)rand.Next(10, 50)) / 10;
      Report report = new Report($"Dep. \"{name}\" has average score of {score}");
      Storage.reports.Add(name, report);
      Console.WriteLine($"Dep. \"{name}\" has sent its report!");

    }
  }

  public class DeansOffice : IObservable {
    private Dictionary<IObserver, string> departments = new Dictionary<IObserver, string>();
    //private List<Report> reports;
    public void Register(string name, IObserver o) {
      departments.Add(o, name);
    }

    public void Remove(IObserver o) {
      departments.Remove(o);
    }
    public void NotifyAll(string msg) {
      foreach(var d in departments) {
        d.Key.Update(msg);
      }
    }

    public void NotifyAboutDeadline() {
      NotifyAll("Your report is required!");
    }

    public void CheckReports() {
      foreach (var d in departments) {
        if (!Storage.reports.ContainsKey(d.Value))
          d.Key.Update("Your report is missing!");
      }
    }
  }

  public class Report {
    public string info { get; private set; }
    public Report(string info) {
      this.info = info;
    }
  }

  public class Storage {
    static public Dictionary<string, Report> reports = new Dictionary<string, Report>();
  }



}
