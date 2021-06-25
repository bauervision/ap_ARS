namespace AMPS_DataModel
{

    #region Base Classes
    [System.Serializable]
    public class BaseTime
    {
        public int epochSeconds;
        public int nanoseconds;

    }

    [System.Serializable]
    public class BaseAsset
    {
        public float value;
        public string units;
    }

    [System.Serializable]
    public class BaseCoordinate
    {
        public string type;
        public double value;
        public string units;
    }

    #endregion

    #region Metadata Related

    [System.Serializable]
    public class Metadata
    {
        public CreatedTime CreatedTime;
        public ModifiedTime ModifiedTime;

    }


    [System.Serializable]
    public class CreatedTime : BaseTime { }

    [System.Serializable]
    public class ModifiedTime : BaseTime { }


    #endregion

    #region Asset Related



    [System.Serializable]
    public class Asset
    {
        public string id;
        public string vehicleId;
        public string name;
        public string environment;
        public Configuration Configuration;

    }

    [System.Serializable]
    public class Configuration
    {
        public string id;
        public string callsign;
        public EmptyWeight EmptyWeight;
        public InternalFuelCapacity InternalFuelCapacity;
        public InternalFuelWeight InternalFuelWeight;
        public FuelBurn FuelBurn;
        public string[] Personnel;
        public SensorInventory[] SensorInventory;
        public string[] CargoAssets;

    }


    [System.Serializable]
    public class EmptyWeight : BaseAsset { }

    [System.Serializable]
    public class InternalFuelCapacity : BaseAsset { }


    [System.Serializable]
    public class InternalFuelWeight : BaseAsset { }

    [System.Serializable]
    public class FuelBurn : BaseAsset { }


    [System.Serializable]
    public class Sensors : BaseAsset { }


    [System.Serializable]
    public class SensorInventory : BaseAsset { }

    #endregion

    #region Points related
    [System.Serializable]
    public class Point
    {
        public string id;
        public string name;
        public Coordinates Coordinates;
    }



    [System.Serializable]
    public class Latitude : BaseCoordinate { }

    [System.Serializable]
    public class Longitude : BaseCoordinate { }


    [System.Serializable]
    public class Coordinates
    {
        public Latitude Latitude;
        public Longitude Longitude;
    }

    #endregion

    #region Route Related

    [System.Serializable]
    public class RoutePoint
    {
        public string id;
        public string pointId;
        public string pointType;

    }

    [System.Serializable]
    public class CargoManifest
    {
        public string id;
        public string[] AssetIds;
        public string[] PersonnelIds;

    }

    [System.Serializable]
    public class Route
    {
        public string id;
        public string name;
        public string assetId;
        //public Asset Asset;
        public Path[] Paths;
        public RoutePoint[] RoutePoints;
        public CargoManifest[] CargoManifests;

    }

    [System.Serializable]
    public class Transition
    {
        public string id;
        public Intent Intent;
    }

    [System.Serializable]
    public class Intent
    {
        public string id;
        public string description;
        public string startPointId;
        public string endPointId;
        public FuelBurn FuelBurn;
        public CommandedAltitude CommandedAltitude;
        public CommandedSpeed CommandedSpeed;
        public TakeoffIntent TakeoffIntent;
    }

    [System.Serializable]
    public class CommandedSpeed : BaseAsset
    {
        public string type;
    }

    [System.Serializable]
    public class TakeoffIntent
    {
        public string launchPlatformType;
    }


    [System.Serializable]
    public class Path
    {
        public string id;
        public string pathType;
        public MinFuel MinFuel;
        public EmergencySafeAltitude EmergencySafeAltitude;
        public InitialState InitialState;
        public Transition[] Transitions;

    }

    [System.Serializable]
    public class MinFuel
    {
        public float value;
        public string units;

    }

    [System.Serializable]
    public class InitialState
    {
        public Altitude Altitude;
        public Time Time;
        public Temperature Temperature;
        public Wind Wind;
    }

    [System.Serializable]
    public class Time : BaseTime { }


    [System.Serializable]
    public class Temperature : BaseAsset { }


    [System.Serializable]
    public class Wind
    {
        public Direction Direction;
        public Speed Speed;
    }


    [System.Serializable]
    public class Direction : BaseAsset
    {
        public string bearingType;
    }

    [System.Serializable]
    public class Speed : BaseAsset
    {
        public string type;
    }



    [System.Serializable]
    public class Altitude : BaseAsset
    {
        public string type;
        public string secondaryUnits;
        public string secondaryValue;
        public string secondaryType;
        public bool isFlightLevel;

    }

    [System.Serializable]
    public class EmergencySafeAltitude : Altitude { }

    [System.Serializable]
    public class CommandedAltitude : Altitude { }



    #endregion

    #region Roster Related
    [System.Serializable]
    public class AlphaRoster
    {
        public Roster[] Rosters;
    }

    [System.Serializable]
    public class Roster
    {
        public string id;
        public string name;
        public Personnel[] PersonnelList;
    }

    [System.Serializable]
    public class Personnel
    {
        public string id;
        public string name;
        public string callsign;
        public string beacon;
        public Equipment[] EquipmentList;
        public Weight Weight;

    }

    [System.Serializable]
    public class Weight : BaseAsset { }


    [System.Serializable]
    public class Equipment
    {
        public string serialNumber;
        public string type;
    }


    #endregion



    [System.Serializable]
    public class Mission
    {

        public string id;
        public string name;
        public string description;
        public string location;
        public double latitude;
        public double longitude;
        public Metadata Metadata;
        public AlphaRoster AlphaRoster;
        public Asset[] Assets;

        public string[] Phases;
        public Point[] Points;
        public Route[] Routes;

    }



    [System.Serializable]
    public class StartTime : BaseTime { }

    [System.Serializable]
    public class MissionData
    {
        public Mission[] missions;
    }


}