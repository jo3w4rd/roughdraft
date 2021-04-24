using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.concept2.com/files/pdf/us/monitors/PM5_BluetoothSmartInterfaceDefinition.pdf
// revision 1.27
public class PM5
{
    //Common
    const int ELAPSED_TIME_LO   = 0;
    const int ELAPSED_TIME_MID  = 1;
    const int ELAPSED_TIME_HIGH = 2;
    const int DISTANCE_LO       = 3;
    const int DISTANCE_MID = 4;
    const int DISTANCE_HIGH     = 5;
    
    // General Status
    const int WORKOUT_TYPE = 6;
    private const int INTERVAL_TYPE = 7;
    private const int WORKOUT_STATE = 8;
    const int ROWING_STATE = 9;
    const int STROKE_STATE = 10;
    private const int WORKOUT_DISTANCE_LO = 11;
    private const int WORKOUT_DISTANCE_MID = 12; 
    private const int WORKOUT_DISTANCE_HIGH = 13;
    private const int WORKOUT_DURATION_LO = 14;
    private const int WORKOUT_DURATION_MID = 15; 
    private const int WORKOUT_DURATION_HIGH = 16;
    private const int WORKOUT_DURATION_TYPE = 17;
    private const int DRAG_FACTOR = 18;

    //Stroke data
    const int DRIVE_LENGTH      = 6;
    const int DRIVE_TIME        = 7;
    const int STROKE_RECOVERY_TIME_LO = 8;
    const int STROKE_RECOVERY_TIME_HIGH = 9;
    const int STROKE_DISTANCE_LO = 10;
    const int STROKE_DISTANCE_HIGH = 11;
    const int PEAK_DRIVE_FORCE_LO = 12;
    const int PEAK_DRIVE_FORCE_HIGH = 13;
    const int AVERAGE_DRIVE_FORCE_LO = 14;
    const int AVERAGE_DRIVE_FORCE_HIGH = 15;
    const int WORK_PER_STROKE_LO = 16;
    const int WORK_PER_STROKE_HIGH = 17;
    const int STROKE_COUNT_LO = 18;
    const int STROKE_COUNT_HIGH = 19;

    //Additional stroke data
    private const int STROKE_POWER_LO = 3;
    private const int STROKE_POWER_MID = 4;
    private const int STROKE_POWER_HIGH = 5;
    private const int CALORIES_LO = 6;
    private const int CALORIES_HIGH = 7;
    private const int ADD_STROKE_COUNT_LO = 8;
    private const int ADD_STROKE_COUNT_HIGH = 9;
    private const int PROJECTED_WORK_TIME_LO = 10;
    private const int PROJECTED_WORK_TIME_MID = 11;
    private const int PROJECTED_WORK_TIME_HIGH = 12;
    private const int PROJECTED_WORK_DISTANCE_LO = 13;
    private const int PROJECTED_WORK_DISTANCE_MID = 14;
    private const int PROJECTED_WORK_DISTANCE_HIGH = 15;


    public struct StrokeData
    {
        public static string UUID = "{ce060035-43e5-11e4-916c-0800200c9a66}";
        public static string name = "C2 rowing stroke data";
        public float ElapsedTime; //seconds
        public float Distance; // meters
        public float DriveLength; // meters
        public float DriveTime; // seconds
        public float StrokeRecoveryTime; // seconds
        public float StrokeDistance; // meters
        public float PeakDriveForce; // lbs of force
        public float AverageDriveForce; // lbs of force
        public float WorkPerStroke; // joules
        public float StrokeCount;

        public StrokeData(byte[] data, bool multiplexed)
        {
            int m = multiplexed ? 1 : 0;
            ElapsedTime = (data[ELAPSED_TIME_LO + m] + (data[ELAPSED_TIME_MID + m] << 8) +
                                                        (data[ELAPSED_TIME_HIGH + m] << 16)) * .01f; //seconds
            Distance = (data[DISTANCE_LO + m] + (data[DISTANCE_MID + m] << 8) + (data[DISTANCE_HIGH + m] << 16)) *
                       .1f; //meters
            DriveLength = data[DRIVE_LENGTH + m] * .01f; //meters 
            DriveTime = data[DRIVE_TIME] * .01f; //seconds
            StrokeRecoveryTime = (data[STROKE_RECOVERY_TIME_LO + m] + 
                                  (data[STROKE_RECOVERY_TIME_HIGH + m] << 8)) * .01f; //seconds
            StrokeDistance = (data[STROKE_DISTANCE_LO + m] + 
                              (data[STROKE_DISTANCE_HIGH + m] << 8)) * .01f; //meters
            PeakDriveForce =
                (data[PEAK_DRIVE_FORCE_LO + m] + (data[PEAK_DRIVE_FORCE_HIGH + m] << 8)) * .1f; //lbs of force
            AverageDriveForce =
                (data[AVERAGE_DRIVE_FORCE_LO + m] + (data[AVERAGE_DRIVE_FORCE_HIGH + m] << 8)) * .1f; //lbs of force
            WorkPerStroke = (data[WORK_PER_STROKE_LO + m] + (data[WORK_PER_STROKE_HIGH + m] << 8)) * .1f; // joules
            StrokeCount = (data[STROKE_COUNT_LO + m] + (data[STROKE_COUNT_HIGH + m] << 8)); //count

        }
    }

    public struct AdditionalStrokeData
    {
        public static string UUID = "{ce060036-43e5-11e4-916c-0800200c9a66}";
        public static string name = "C2 rowing additional stroke data";
        public float ElapsedTime; // seconds
        public float StrokePower; //watts
        public float Calories; // cal/hr
        public int StrokeCount;
        public float ProjectedWorkTime; // seconds
        public float ProjectedWorkDistance; // meters
            
        public AdditionalStrokeData(byte[] data, bool multiplexed)
        {
            int m = multiplexed ? 1 : 0;
            ElapsedTime = (data[ELAPSED_TIME_LO + m] + (data[ELAPSED_TIME_MID + m] << 8) +
                           (data[ELAPSED_TIME_HIGH + m] << 16)) * .01f; //seconds
            StrokePower = (data[STROKE_POWER_LO + m] + (data[STROKE_POWER_MID + m] << 8) + (data[STROKE_POWER_HIGH + m] << 16)); //Watts
            Calories = (data[CALORIES_LO + m] + (data[CALORIES_HIGH + m] << 8)); // cal/hr
            StrokeCount = (data[ADD_STROKE_COUNT_LO + m] + (data[ADD_STROKE_COUNT_HIGH + m] << 8)); //count
            ProjectedWorkTime = (data[PROJECTED_WORK_TIME_LO + m] + (data[PROJECTED_WORK_TIME_MID + m] << 8) + (data[PROJECTED_WORK_TIME_HIGH + m] << 16)); //Watts
            ProjectedWorkDistance = (data[PROJECTED_WORK_DISTANCE_LO + m] + (data[PROJECTED_WORK_DISTANCE_MID + m] << 8) + (data[PROJECTED_WORK_DISTANCE_HIGH + m] << 16)); //Watts
        }
    }
    public struct GeneralStatus
    {
        public static string UUID = "{ce060031-43e5-11e4-916c-0800200c9a66}";
        public static string name = "C2 rowing general status";
        public float ElapsedTime; // seconds
        public float Distance; // meters
        public TypeWorkout WorkoutType; // CSAFE_PM_GET_WORKOUTTYPE4
        public TypeInterval IntervalType; // CSAFE_PM_GET_INTERVALTYPE
        public TypeWorkoutState WorkoutState; // CSAFE_PM_GET_WORKOUTSTATE
        public TypeRowingState RowingState; // CSAFE_PM_GET_ROWINGSTATE
        public TypeStrokeState StrokeState; // CSAFE_PM_GET_STROKESTATE
        public float TotalWorkoutDistance; // meters
        public float WorkoutDuration; // seconds
        public TypeWorkoutDuration WorkoutDurationType; // CSAFE_PM_GET_WORKOUTDURATION
        public int DragFactor; // unitless, CSAFE_PM_GET_DRAGFACTOR
        public GeneralStatus(byte[] data, bool multiplexed)
        {
            int m = multiplexed ? 1 : 0;
            ElapsedTime = (data[ELAPSED_TIME_LO + m] + (data[ELAPSED_TIME_MID + m] << 8) +
                           (data[ELAPSED_TIME_HIGH + m] << 16)) * .01f; //seconds
            Distance = (data[DISTANCE_LO + m] + (data[DISTANCE_MID + m] << 8) + (data[DISTANCE_HIGH + m] << 16)) *
                       .1f; //meters

            WorkoutType = (TypeWorkout)data[WORKOUT_TYPE + m];
            IntervalType = (TypeInterval)data[INTERVAL_TYPE + m];
            WorkoutState = (TypeWorkoutState)data[WORKOUT_STATE + m];
            RowingState = (TypeRowingState)data[ROWING_STATE + m];
            StrokeState = (TypeStrokeState)data[STROKE_STATE + m];
            TotalWorkoutDistance = (data[WORKOUT_DISTANCE_LO + m] + (data[WORKOUT_DISTANCE_MID + m] << 8) + (data[WORKOUT_DISTANCE_HIGH + m] << 16)) *
                                   .1f; //meters
            WorkoutDuration = (data[WORKOUT_DURATION_LO + m] + (data[WORKOUT_DURATION_MID + m] << 8) +
                               (data[WORKOUT_DURATION_HIGH + m] << 16)); // either distance or time
            WorkoutDurationType = (TypeWorkoutDuration)data[WORKOUT_DURATION_TYPE + m];
            if (WorkoutDurationType == TypeWorkoutDuration.TIME) 
            {
                WorkoutDuration *= .01f; // seconds
            }

            DragFactor = data[DRAG_FACTOR]; // unitless
        }
        public override  string ToString()
        {
            return 
                $"Elapsed time:   {ElapsedTime} seconds\n" +
                $"Distance:       {Distance} meters\n" +
                $"Workout type:   {WorkoutType}\n" +
                $"Interval type:  {IntervalType}\n" +
                $"Workout state:  {WorkoutState}\n" +
                $"Rowing state:   {RowingState}\n" +
                $"Stroke state:   {StrokeState}\n" +
                $"Total distance: {TotalWorkoutDistance} meters\n" +
                $"Duration:       {WorkoutDuration}\n" +
                $"Duration type:  {WorkoutDurationType}\n" +
                $"Drag factor:    {DragFactor}\n";
        }
    }
    
     public enum TypeWorkout : int {
        JUSTROW_NOSPLITS = 0,
        JUSTROW_SPLITS,
        FIXEDDIST_NOSPLITS,
        FIXEDDIST_SPLITS,
        FIXEDTIME_NOSPLITS,
        FIXEDTIME_SPLITS,
        FIXEDTIME_INTERVAL,
        FIXEDDIST_INTERVAL,
        VARIABLE_INTERVAL,
        VARIABLE_UNDEFINED_REST_INTERVAL,
        FIXED_CALORIE,
        FIXED_WATTMINUTES,
        FIXEDCALS_INTERVAL,
        NUM
    }
     public enum TypeInterval : int {
         TIME,
         DISTANCE,
         REST,
         TIME_REST_UNDEFINED,
         DISTANCE_REST_UNDEFINED,
         REST_UNDEFINED,
         CAL,
         CAL_REST_UNDEFINED,
         WATT_MINUTE,
         WATT_MINUTE_REST_UNDEFINED,
         NONE = 255
     }
     
     public enum TypeWorkoutState : int {
         WAIT_TO_BEGIN = 0,
         WORKOUT_ROW = 1,
         COUNTDOWN_PAUSE = 2,
         INTERVAL_REST = 3,
         INTERVAL_WORK_TIME = 4,
         INTERVAL_WORK_DISTANCE = 5,
         INTERVAL_REST_END_TO_WORK_TIME = 6,
         INTERVAL_REST_END_TO_WORK_DISTANCE = 7,
         INTERVAL_WORK_TIME_TO_REST = 8,
         INTERVAL_WORK_DISTANCE_TO_REST = 9,
         WORKOUT_END = 10,
         TERMINATE = 11,
         WORKOUT_LOGGED = 12,
         REARM = 13
     }
     
     public enum TypeRowingState : int {
         INACTIVE,
         ACTIVE
     }

     public enum TypeStrokeState : int{
         WAITING_FOR_WHEEL_TO_REACH_MIN_SPEED_STATE,
         WAITING_FOR_WHEEL_TO_ACCELERATE_STATE,
         DRIVING_STATE,
         DWELLING_AFTER_DRIVE_STATE,
         RECOVERY_STATE
     };
     
     public enum TypeWorkoutDuration {
         TIME = 0,
         CALORIES = 0X40,
         DISTANCE = 0X80,
         WATTS = 0XC0
     } 

     public enum TypeGameID {
         GAMEID_NONE,
         GAMEID_FISH,
         GAMEID_DART,
         GAMEID_TARGET_BASIC,
         GAMEID_TARGET_ADVANCED,
         GAMEID_CROSSTRAINING
     }
}

