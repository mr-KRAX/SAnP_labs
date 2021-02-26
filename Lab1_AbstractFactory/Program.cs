using System;
using System.Collections.Generic;

namespace Lab1_AbstractFactory {
  class Program {
    static void Main(string[] args) {
      Trip bus_trip = new Trip(new BusTripFactory());
      Trip taxi_trip = new Trip(new TaxiTripFactory());

      bus_trip.TakeOff();
      Console.WriteLine("");
      taxi_trip.TakeOff();
    }
  }

  class Trip {
    static int lastTripNum = 0;
    public int num { get; protected set; }
    public Driver driver { get; protected set; }
    public Vehicle vehicle { get; protected set; }
    public List<Passenger> passengers { get; protected set; }

    private AbstractTripFactory factory;

    public Trip(AbstractTripFactory factory) {
      this.num = lastTripNum++;
      this.factory = factory;
      this.passengers = new List<Passenger>();

      driver = factory.CreateDriver();
      vehicle = factory.CreateVehicle();

      for (int i = 0; i < vehicle.sits; i++) {
        passengers.Add(factory.CreatePassenger());
      }
    }

    public void TakeOff() {
      Console.WriteLine($"Initiate the Trip#{num}.");
      foreach (var p in passengers) {
        p.PayForTicket();
      }
      Console.WriteLine("Everyone paid, driver was allowed to go.");
      driver.Drive();
      Console.WriteLine("Some passenger started complaining!");
      Random rnd = new Random();
      passengers[rnd.Next(vehicle.sits)].Complain();
      Console.WriteLine("The vehicle rode off into the sunset");
    }

  }

  #region FACTORY
  abstract class AbstractTripFactory {
    public abstract Passenger CreatePassenger();
    public abstract Vehicle CreateVehicle();
    public abstract Driver CreateDriver();
  }

  class BusTripFactory : AbstractTripFactory {
    static int lastDriverId = 0;
    static int lastPassengerId = 0;
    static PassengerType lastPassengerType = PassengerType.GENERAL;
    static int lastVehicleId = 0;
    public override Driver CreateDriver() {
      lastDriverId += 2;
      return new BusDriver(lastDriverId);
    }

    public override Passenger CreatePassenger() {
      Passenger p = new BusPassenger($"BusPassenger#{lastPassengerId++}");
      Random rnd = new Random();
      lastPassengerType = (PassengerType)rnd.Next(3);
      p.type = lastPassengerType;
      return p;
    }

    public override Vehicle CreateVehicle() {
      return new BusVehicle($"BUS#{lastVehicleId++}");
    }
  }

  class TaxiTripFactory : AbstractTripFactory {
    static int lastDriverId = 1;
    static int lastPassengerId = 0;
    static int lastVehicleId = 0;
    public override Driver CreateDriver() {
      lastDriverId += 2;
      return new BusDriver(lastDriverId);
    }

    public override Passenger CreatePassenger() {
      return new TaxiPassenger($"TaxiPassenger#{lastPassengerId++}");
    }

    public override Vehicle CreateVehicle() {
      return new TaxiVehicle($"TAXI#{lastVehicleId++}");
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
      Console.WriteLine("There's so mush ppl in here!");
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
      Console.WriteLine("The taxi is an expensive transport!");
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
      Console.WriteLine("I'm driving a taxi!");
    }
  }
  class BusDriver : Driver {
    public BusDriver(int id) : base(id) { }

    public override void Drive() {
      Console.WriteLine("I'm driving the bus!");
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
