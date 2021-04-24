using System;
using System.Collections.Generic;

namespace Lab2_Builder {
  class Program {
    static void Main(string[] args) {
      Director dir = new Director();

      Trip bus_trip = dir.createTrip(new BusTripBuilder(), 10);
      Trip taxi_trip = dir.createTrip(new TaxiTripBuilder(), 4);
      Trip coach_trip = dir.createTrip(new CoachTripBuilder(), 30);

      Trip bus_trip2 = dir.createTrip(new BusTripBuilder(), 15);
      Trip taxi_trip2 = dir.createTrip(new TaxiTripBuilder(), 4);
      Trip coach_trip2 = dir.createTrip(new CoachTripBuilder(), 30);

      Trip bus_trip3 = dir.createTrip(new BusTripBuilder(), 19);
      Trip taxi_trip3 = dir.createTrip(new TaxiTripBuilder(), 2);
      Trip coach_trip3 = dir.createTrip(new CoachTripBuilder(), 10);


      bus_trip.TakeOff();
      Console.WriteLine("--------------------------------");
      taxi_trip.TakeOff();
      Console.WriteLine("--------------------------------");
      coach_trip.TakeOff();
      Console.WriteLine("--------------------------------");

      bus_trip2.TakeOff();
      Console.WriteLine("--------------------------------");
      taxi_trip2.TakeOff();
      Console.WriteLine("--------------------------------");
      coach_trip2.TakeOff();
      Console.WriteLine("--------------------------------");

      bus_trip3.TakeOff();
      Console.WriteLine("--------------------------------");
      taxi_trip3.TakeOff();
      Console.WriteLine("--------------------------------");
      coach_trip3.TakeOff();
      Console.WriteLine("--------------------------------");


    }
  }
  
  class Trip {
    static int currTripNum = 0;
    public int num { get; protected set; }
    public Driver[] driver { get; set; }
    public Vehicle vehicle { get; set; }
    public List<Passenger> passengers { get; set; }
    public bool safetySeatNeeded { get; set; }

    public Trip() {
      this.num = currTripNum++;
      this.safetySeatNeeded = false;
    }

    public void TakeOff() {
      Console.WriteLine($"Initiate the Trip#{num}.");
      if (driver.Length == 0) {
        Console.WriteLine("No drivers inside! Diparture is abolished!");
        return;
      }
      if (safetySeatNeeded)
        Console.WriteLine("Safety seat added!");
      foreach (var p in passengers) {
        p.PayForTicket();
      }
      Console.WriteLine("Everyone paid, driver was allowed to go.");
      driver[0].Drive();
      Console.WriteLine("Some passenger started complaining!");
      Random rnd = new Random();
      passengers[rnd.Next(passengers.Count)].Complain();
      Console.WriteLine("The vehicle rode off into the sunset");
    }

  }

  class Director {
    public Trip createTrip(AbstractTripBuilder b, int passengersNum) {
      b.createTrip();
      b.BuildVehicle();
      b.BuildDriver();
      for (int i = 0; i < passengersNum; i++) {
        b.BuildPassenger();
      }
      b.BuildSafetySeat();
      return b.getTrip();
    }
  }


  #region BUILDER
  abstract class AbstractTripBuilder {
    protected Trip t;

    public AbstractTripBuilder() { t = null; }
    public virtual void createTrip() { }
    public virtual void BuildPassenger() { }
    public virtual void BuildVehicle() { }
    public virtual void BuildDriver() { }
    public virtual void BuildSafetySeat() { }

    public Trip getTrip() { return t; }
  }

  class BusTripBuilder : AbstractTripBuilder {
    static int lastDriverId = 0;
    static int lastPassengerId = 0;
    static PassengerType lastPassengerType = PassengerType.GENERAL;
    static int lastVehicleId = 0;

    public override void createTrip() {
      t = new Trip();
      t.driver = new Driver[1];
      t.driver[0] = null;
      t.vehicle = null;
      t.passengers = new List<Passenger>();
    }
    public override void BuildDriver() {
      lastDriverId += 2;
      if (t.driver[0] != null && t.driver[1] != null)
        throw new Exception("No more than two drivers allowed!");
      t.driver[0] = new BusDriver(lastDriverId);
    }

    public override void BuildPassenger() {
      if (t.passengers.Count == t.vehicle.sits)
        throw new Exception("Too much ppl ({t.passengers.Count+1}) for the vehicle of type Coach");
      Passenger p = new BusPassenger($"BusPassenger#{lastPassengerId++}");
      Random rnd = new Random();
      lastPassengerType = (PassengerType)rnd.Next(3);
      p.type = lastPassengerType;
      t.passengers.Add(p);
    }

    public override void BuildVehicle() {
      if (t.vehicle != null)
        throw new Exception("Only one vehicle allowed!");
      t.vehicle = new BusVehicle($"BUS#{lastVehicleId++}");
    }
  }

  class TaxiTripBuilder : AbstractTripBuilder {
    static int lastDriverId = 1;
    static int lastPassengerId = 0;
    static PassengerType lastPassengerType = PassengerType.GENERAL;
    static int lastVehicleId = 0;

    public override void createTrip() {
      t = new Trip();
      t.driver = new Driver[1];
      t.driver[0] = null;
      t.vehicle = null;
      t.passengers = new List<Passenger>();
    }
    public override void BuildDriver() {
      lastDriverId += 2;
      if (t.driver[0] != null)
        throw new Exception("Only one driver allowed!");
      t.driver[0] = new TaxiDriver(lastDriverId);
    }

    public override void BuildPassenger() {
      if (t.passengers.Count == t.vehicle.sits)
        throw new Exception("Too much ppl ({t.passengers.Count+1}) for the vehicle of type Taxi");
      Passenger p = new TaxiPassenger($"BusPassenger#{lastPassengerId++}");
      Random rnd = new Random();
      lastPassengerType = (PassengerType)rnd.Next(3);
      p.type = lastPassengerType;
      t.passengers.Add(p);
    }

    public override void BuildVehicle() {
      if (t.vehicle != null)
        throw new Exception("Only one vehicle allowed!");
      t.vehicle = new TaxiVehicle($"TAXI#{lastVehicleId++}");
    }
    public override void BuildSafetySeat() {
      foreach (var p in t.passengers)
        if (p.type == PassengerType.CHILD) {
          t.safetySeatNeeded = true;
        }
    }
  }

  class CoachTripBuilder : AbstractTripBuilder {
    static int lastDriverId = 0;
    static int lastPassengerId = 0;
    static PassengerType lastPassengerType = PassengerType.GENERAL;
    static int lastVehicleId = 0;

    public override void createTrip() {
      t = new Trip();
      t.driver = new Driver[2];
      t.driver[0] = null;
      t.driver[1] = null;
      t.vehicle = null;
      t.passengers = new List<Passenger>();
    }
    public override void BuildDriver() {
      lastDriverId += 2;
      if (t.driver[0] != null)
        throw new Exception("Only one driver allowed!");
      t.driver[0] = new BusDriver(lastDriverId);
      lastDriverId += 2;
      t.driver[1] = new BusDriver(lastDriverId);
    }

    public override void BuildPassenger() {
      if (t.passengers.Count == t.vehicle.sits)
        throw new Exception("Too much ppl ({t.passengers.Count+1}) for the vehicle of type Bus");
      Passenger p = new BusPassenger($"BusPassenger#{lastPassengerId++}");
      Random rnd = new Random();
      lastPassengerType = (PassengerType)rnd.Next(3);
      p.type = lastPassengerType;
      t.passengers.Add(p);
    }

    public override void BuildVehicle() {
      if (t.vehicle != null)
        throw new Exception("Only one vehicle allowed!");
      t.vehicle = new BusVehicle($"BUS#{lastVehicleId++}");
    }

    public override void BuildSafetySeat() {
      foreach (var p in t.passengers)
        if (p.type == PassengerType.CHILD) {
          t.safetySeatNeeded = true;
        }
    }
  }
  #endregion

  #region PASSENGER
  enum PassengerType {
    CHILD,
    GENERAL,
    OLD
  }
  abstract class Passenger {
    public string name { get; protected set; }
    public PassengerType type { get; set; }

    public Passenger(string name) {
      this.name = name;
      this.type = PassengerType.GENERAL;
    }
    public abstract void Complain();
    public abstract void PayForTicket();
  }
  class BusPassenger : Passenger {
    public BusPassenger(string name) :
      base(name) { }
    public override void Complain() {
      Console.WriteLine("P: There's so mush ppl in here!");
    }
    public override void PayForTicket() {
      string price = "";
      switch (this.type) {
        case PassengerType.CHILD:
          price = "50";
          break;
        case PassengerType.GENERAL:
          price = "100";
          break;
        case PassengerType.OLD:
          price = "0";
          break;
        default:
          break;
      }
      Console.WriteLine("Passenger pays: " + price + "% of the ticket price");
    }
  }
  class TaxiPassenger : Passenger {
    public TaxiPassenger(string name) : base(name) { }
    public override void Complain() {
      Console.WriteLine("P: The taxi is an expensive transport!");
    }
    public override void PayForTicket() {
      Console.WriteLine("Passenger pays: full price, no mercy");
    }
  }
  #endregion

  #region DRIVER
  abstract class Driver {
    public int id { get; protected set; }

    public Driver(int id) {
      this.id = id;
    }

    public abstract void Drive();
  }
  class TaxiDriver : Driver {
    public TaxiDriver(int id) : base(id) { }

    public override void Drive() {
      Console.WriteLine("D: I'm driving a taxi!");
    }
  }
  class BusDriver : Driver {
    public BusDriver(int id) : base(id) { }

    public override void Drive() {
      Console.WriteLine("D: I'm driving the bus!");
    }
  }
  #endregion

  #region VEHICLE
  abstract class Vehicle {
    public string id { get; protected set; }
    public abstract int sits { get; }

    public Vehicle(string id) {
      this.id = id;
    }
  }
  class BusVehicle : Vehicle {
    public BusVehicle(string id) : base(id) { }

    public override int sits => 30;
  }
  class TaxiVehicle : Vehicle {
    public TaxiVehicle(string id) : base(id) { }

    public override int sits => 4;
  }
  #endregion
}
