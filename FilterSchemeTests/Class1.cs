using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using NUnit.Framework;
using Orc.FilterBuilder;
using Orc.FilterBuilder.Models;
using Catel.Data;
using Orc.FileSystem;
using Catel.Runtime.Serialization.Xml;
using Catel.IoC;
using Catel.Runtime.Serialization;

[TestFixture()]
[Category("Filter")]
public class FilterTests
{
    
    [Test()]
    [Category("Save")]
    public void OrcFilterSaveScenario()
    {
        Type targetType = typeof(DataPointFilter);
        FilterScheme filterscheme = new FilterScheme(targetType, "Alle Daten");

        PropertyExpression pe = new PropertyExpression();
        var iex = new EnumExpression<Phase>(false);
        var pManager = new InstanceProperties(targetType);

        // Dim pi As New PropertyData

        iex.SelectedCondition = Condition.EqualTo;
        iex.Value = Phase.Isokinetic;
        pe.Property = pManager.GetProperty("Phase");
        pe.DataTypeExpression = iex;

        var cGroup = new ConditionGroup() { Type = ConditionGroupType.And };
        cGroup.Items.Add(pe);

        filterscheme.Root.Items.Add(cGroup);
        // filterscheme.ConditionItems.Add(cGroup)

        FilterSchemes fSchemes = new FilterSchemes();
        fSchemes.Schemes.Add(filterscheme);
        const string FilePath = @"C:\Temp\filters.xml";
        fSchemes.SaveAsXml(FilePath);

        Assert.IsTrue(System.IO.File.Exists(FilePath));
        Assert.Greater(new System.IO.FileInfo(FilePath).Length, 0);
    }
}

/// <summary>

/// ''' Ein Datenpunkt einer Isomed-Nessung/Training

/// ''' </summary>

/// ''' <remarks>Die Werte sind entsprechend den Vorgaben konvertiert und in die 

/// ''' nächstliegende SI konvertiert</remarks>
public class DataPointFilter
{
    /// <summary>
    ///     ''' Index für Array-Zugriff und DB
    ///     ''' </summary>
    ///     ''' <remarks></remarks>
    public int Id { get; set; }
    /// <summary>
    ///     ''' Bezeichnet die Zeilennr in der Rohdatei des Messpunktes 
    ///     ''' </summary>
    ///     ''' <remarks></remarks>
    public int Row { get; set; }
    /// <summary>
    ///     ''' Zeitstempel des Datenpunkts in Sekunden
    ///     ''' </summary>
    ///     ''' <remarks></remarks>
    public double Time { get; set; }
    /// <summary>
    ///     ''' Relative Position in Meter zum Nullpunkt
    ///     ''' </summary>
    ///     ''' <remarks></remarks>
    public double Position { get; set; }
    /// <summary>
    ///     ''' Resultierendes Drehmoment in Nm bzw. resultierende Kraft in N
    ///     ''' </summary>
    ///     ''' <remarks></remarks>
    public double Torque { get; set; }
    /// <summary>
    ///     ''' Geschwindigkeit in °/sek bzw. m/sek
    ///     ''' </summary>
    ///     ''' <remarks></remarks>
    public double Speed { get; set; }
    public double TorqueWithoutGravity { get; set; }
    // Public Property Repetition As Integer
    public int Set { get; set; }
    public double TorqueOnDyno { get; set; }
    public double ForceRight { get; set; }
    public double ForceLeft { get; set; }
    public Movement Movement { get; set; }
    public int RepPerSet { get; set; }
    public Side Side { get; set; }
    public Phase Phase { get; set; }
    public double Work { get; set; }
    public double Acceleration { get; set; }
    public double Power { get; set; }
    public byte RepChange { get; set; }
    public int RepetionRaw { get; set; }
    public Contraction Contraction { get; set; }

    public int Repetition { get; set; }
}


public enum Contraction
{
    concentric = 0,
    excentric
}

public enum TestMode
{
    kon_kon = 1,
    kon_exz,
    exz_kon,
    exz_exz,
    isometrisch = 6,
    aktiv_assistiv,
    stat_koord_Stabilisation = 12,
    athletic = 21
}

public enum Movement : byte
{
    B1 = 0,
    B2 = 1
}

[Flags()]
public enum Side : byte
{
    Right = 1,
    Left,
    Both
}

public enum Phase : ushort
{
    Acceleration = 0,
    Isokinetic,
    Deceleration,
    Rest
}

public enum ContractionMode
{
    Undefiniert = -1,
    Statisch,
    Konz_Konz,
    Exz_Konz,
    Konz_Exz,
    Exz_Exz
}

