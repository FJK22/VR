﻿using System.Collections.Generic;

public class Constant
{
    public const string DOMAIN = @"https://www.oxforia.com/api/apifjm82/vrenvironments/";
    public const string Level = @"common/save_level";
    public const string Clear = @"common/clear_data";
    public const string USER = @"user/save_user";
    public const string SC1Data = @"sc1data/save_sc1data";
    public const string SC1QS = @"sc1qs/save_sc1qs";
    public const string SC2Data = @"sc2data/save_sc2data";
    public const string SC2BData = @"sc2bdata/save_sc2bdata";
    public const string SC2QS = @"sc2qs/save_sc2qs";
    public const string SC3AData = @"sc3adata/save_sc3adata"; 
    public const string SC3BData = @"sc3bdata/save_sc3bdata";
    public const string SC3QS = @"sc3qs/save_sc3qs";
    public const string SC4QS = @"sc4qs/save_sc4qs";
    public const string SC5Data = @"sc5data/save_sc5data";
    public const string SC5QS = @"sc5qs/save_sc5qs";
    public const string SC6Data = @"sc6data/save_sc6data";
    public const string SC6QS = @"sc6qs/save_sc6qs";
    public const string SC7Data = @"sc7data/save_sc7data"; 
    public const string SC7QS = @"sc7qs/save_sc7qs ";
    public const string SC8Data = @"sc8data/save_sc8data ";
    public const string SC8QS = @"sc8qs/save_sc8qs ";
}
public enum SceneType
{
    Sc1Questionnaire,
    Sc2LectureHall,
    Sc2BLectureHall,
    Sc2Questionnaire,
    Sc3AStreet,
    Sc3BStreet,
    Sc3Questionnaire,
    Sc4Bar,
    Sc4Questionnaire,
    Sc5StreetPedestrian,
    Sc5Questionnaire,
    Sc6TrainStation,
    Sc6Questionnaire,
    Sc7Elevator,
    Sc7Questionnaire,
    Sc8ChemistryLab,
    Sc8Questionnaire,
    End
}