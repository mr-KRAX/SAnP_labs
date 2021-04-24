using System;
using System.Collections.Generic;

namespace Lab3_Composite {
  class Program {
    static void Main(string[] args) {

      Plain plain = new Plain();

      for (int i = 0; i < 2; i++)
        plain.addUnit(new Pilot());

      for (int i = 0; i < 6; i++)
        plain.addUnit(new FlightAttendant());

      for (int i = 0; i < 10; i++)
        plain.addUnit(new FirstClassPassenger());

      for (int i = 0; i < 20; i++)
        plain.addUnit(new BusinessClassPassenger());

      for (int i = 0; i < 150; i++)
        plain.addUnit(new EconomClassPassenger());

      plain.printStatistics();

      Console.WriteLine("\n\nRemove passenger 15\n\n");
      plain.removePassenger(15);


      plain.printStatistics();



    }
  }

  class Unit {
    protected int weight = 0;
    virtual public int getWeight() {
      return weight;
    }
  }

  class CompositUnit : Unit {
    private List<Unit> units = new List<Unit>();
    override public int getWeight() {
      int sum = 0;
      foreach (var u in units) {
        sum += u.getWeight();
      }
      return sum;
    }
    public void addUnit(Unit u) {
      units.Add(u);
    }

    public void removeUnit(int index) {
      units.RemoveAt(index);
    }
    public Unit getUnit(int index) {
      return units[index];
    }

    public int count() {
      return units.Count;
    }
  }


  abstract class Passenger : Unit {
    static int lastId = 0;
    public int id { get; private set; }
    public string seat { get; set; }
    
    public Passenger() {
      id = lastId++;
    }
  }

  class EconomClassPassenger : Passenger {
    public EconomClassPassenger() : base() {
      Random rnd = new Random();
      weight = rnd.Next(5, 21);
    }
    public void cancelLuggage() {
      weight = 0;
    }
  }

  class BusinessClassPassenger : Passenger {
    public BusinessClassPassenger() {
      Random rnd = new Random();
      weight = rnd.Next(5, 36);
    }
  }

  class FirstClassPassenger : Passenger {
    public FirstClassPassenger() {
      Random rnd = new Random();
      weight = rnd.Next(5, 150);
    }
  }

  class Pilot : Unit {
    public override int getWeight() {
      return 0;
    }
  }

  class FlightAttendant : Unit {
    public override int getWeight() {
      return 0;
    }
  }

  class Plain : CompositUnit{
    int maxWeight = 1000;
    const int maxPilots = 2;
    const int maxFlightAttendants = 6;
    const int maxFirstPassengers = 10;
    const int maxBusinessPassengers = 20;
    const int maxEconomPassengers = 150;
    private List<string> freeFirstSeats = new List<string>();
    private List<string> freeEconomSeats = new List<string>();
    private List<string> freeBusinessSeats = new List<string>();
    public Plain() {
      freeFirstSeats = new List<string>();
      for (int i = 0; i < maxFirstPassengers; i++)
        freeFirstSeats.Add($"F{i}");
      for (int i = 0; i < maxBusinessPassengers; i++)
        freeBusinessSeats.Add($"B{i}");
      for (int i = 0; i < maxEconomPassengers; i++)
        freeEconomSeats.Add($"E{i}");

      addUnit(new CompositUnit());
      addUnit(new CompositUnit());

      addUnit(new CompositUnit());
      addUnit(new CompositUnit());
      addUnit(new CompositUnit());

      //addUnit(createPilotsUnit());
      //addUnit(createFlightAttendantsUnits());
      //addUnit(createFirstClass());
      //addUnit(createBusinessClass());
      //addUnit(createEconomClass());
    }

    public void printStatistics() {
      Console.WriteLine("Flight");
      Console.WriteLine($"Total weight {getWeight()}, overweight: {(getWeight() > maxWeight ? "YES" : "NO")} ");
      CompositUnit firstClass = getUnit(2) as CompositUnit;
      Console.Write("\n\nFirst: ");
      for (int i = 0; i < firstClass.count(); i++) {
        Passenger p = firstClass.getUnit(i) as Passenger;
        Console.Write($"({p.id}, {p.seat}, {(p.getWeight() == 0 ? "N" : p.getWeight().ToString())}) ");
      }

      CompositUnit businessClass = getUnit(3) as CompositUnit;
      Console.Write("\n\nBusiness: ");
      for (int i = 0; i < businessClass.count(); i++) {
        Passenger p = businessClass.getUnit(i) as Passenger;
        Console.Write($"({p.id}, {p.seat}, {(p.getWeight() == 0 ? "N" : p.getWeight().ToString())}) ");
      }

      CompositUnit economClass = getUnit(4) as CompositUnit;
      Console.Write("\n\nEconom: ");
      for (int i = 0; i < economClass.count(); i++) {
        Passenger p = economClass.getUnit(i) as Passenger;
        Console.Write($"({p.id}, {p.seat}, {(p.getWeight()==0 ? "N" : p.getWeight().ToString())}) ");
      }
    }

    public void addUnit(Pilot p) {
      CompositUnit cu = getUnit(0) as CompositUnit;
      if (cu.count() >= maxPilots)
        throw new Exception("Too much pilots");
      cu.addUnit(p);
    }
    public void addUnit(FlightAttendant fa) {
      CompositUnit cu = getUnit(1) as CompositUnit;
      if (cu.count() >= maxFlightAttendants)
        throw new Exception("Too much flight attendants");
      cu.addUnit(fa);
    }
    public void removePassenger(int passId) {
      CompositUnit firstClass = getUnit(2) as CompositUnit;
      for(int i = 0; i < firstClass.count(); i++) {
        Passenger p = (Passenger)firstClass.getUnit(i);
        if (p.id == passId) {
          firstClass.removeUnit(i);
          freeFirstSeats.Add(p.seat);
          return;
        }
      }
      CompositUnit businessClass = getUnit(3) as CompositUnit;
      for (int i = 0; i < businessClass.count(); i++) {
        Passenger p = (Passenger)businessClass.getUnit(i);
        if (p.id == passId) {
          businessClass.removeUnit(i);
          freeBusinessSeats.Add(p.seat);
          return;
        }
      }
      CompositUnit economClass = getUnit(4) as CompositUnit;
      for (int i = 0; i < economClass.count(); i++) {
        Passenger p = (Passenger)economClass.getUnit(i);
        if (p.id == passId) {
          economClass.removeUnit(i);
          freeEconomSeats.Add(p.seat);
          return;
        }
      }

    }

    public void addUnit(FirstClassPassenger p) {
      CompositUnit cu = getUnit(2) as CompositUnit;
      if (cu.count() >= maxFirstPassengers)
        throw new Exception("Too much first passengers");
      p.seat = freeFirstSeats[0];
      freeFirstSeats.Remove(p.seat);
      cu.addUnit(p);
    }

    public void addUnit(BusinessClassPassenger p) {
      CompositUnit cu = getUnit(3) as CompositUnit;
      if (cu.count() >= maxBusinessPassengers)
        throw new Exception("Too much business passengers");
      p.seat = freeBusinessSeats[0];
      freeBusinessSeats.Remove(p.seat);
      cu.addUnit(p);
    }

    public void addUnit(EconomClassPassenger p) {
      CompositUnit cu = getUnit(4) as CompositUnit;
      if (cu.count() >= maxEconomPassengers)
        throw new Exception("Too much econom passengers");
      p.seat = freeEconomSeats[0];
      freeEconomSeats.Remove(p.seat);
      if (getWeight() > maxWeight)
        p.cancelLuggage();
      cu.addUnit(p);
    }

    //private CompositUnit createFlightAttendantsUnits() {
    //  CompositUnit cu = new CompositUnit();
    //  for (int i = 0; i < 6; i++)
    //    cu.addUnit(new FlightAttendant());
    //  return cu;
    //}
    //private CompositUnit createPilotsUnit() {
    //  CompositUnit cu = new CompositUnit();
    //  cu.addUnit(new Pilot());
    //  cu.addUnit(new Pilot());
    //  return cu;

    //}
    //private CompositUnit createBusinessClass() {
    //  CompositUnit cu = new CompositUnit();
    //  for (int i = 0; i < 20; i++) {
    //    BusinessClassPassenger p = new BusinessClassPassenger();
    //    cu.addUnit(p);
    //  }
    //  return cu;
    //}
    //private CompositUnit createFirstClass() {
    //  CompositUnit cu = new CompositUnit();
    //  for (int i = 0; i < 10; i++) {
    //    FirstClassPassenger p = new FirstClassPassenger();
    //    cu.addUnit(p);
    //  }
    //  return cu;
    //}
    //private CompositUnit createEconomClass() {
    //  CompositUnit cu = new CompositUnit();
    //  for (int i = 0; i < 150; i++) {
    //    EconomClassPassenger p = new EconomClassPassenger();
    //    if (getWeight() + p.getWeight() < maxWeight)
    //      p.cancelLuggage();
    //    cu.addUnit(p);
    //  }
    //  return cu;
    //}
  }
}
