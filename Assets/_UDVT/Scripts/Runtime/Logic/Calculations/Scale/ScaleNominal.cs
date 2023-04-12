using System;
using System.Collections.Generic;
using UnityEngine;

public class ScaleNominal:Scale
{
    private List<string> scaledValueNames;

    private int domainValues = 5;

    private double rangeMin = 0.0d;
    private double rangeMax = 1.0d;

    public ScaleNominal(List<double> domain, List<double> range, List<string> names) : base(domain, range)
    {
        dataScaleType = DataScaleType.Nominal;

        domainValues = Convert.ToInt32(domain[1] - domain[0]);

        if (domainValues == names.Count || domain[0] != 0)
        {
            Debug.LogError("Not enough Names for the domain entered");
            return;
        }

        rangeMin = range[0];
        rangeMax = range[1];

        scaledValueNames = names;
    }

    public ScaleNominal(List<double> domain, List<string> names) : base(domain)
    {
        dataScaleType = DataScaleType.Nominal;

        domainValues = Convert.ToInt32(domain[1] - domain[0]);

        if (domainValues == names.Count || domain[0] != 0)
        {
            Debug.LogError("Not enough Names for the domain entered");
            return;
        }

        rangeMin = range[0];
        rangeMax = range[1];

        scaledValueNames = names;
    }


    public override double GetScaledValue(double domainValue)
    {
        return domainValue * ((rangeMax - rangeMin) / domainValues) + rangeMin;
    }

    public override double GetDomainValue(double scaledValue)
    {
        //TODO: Rework to not use double (with int rounding problems obsolete)
        return Math.Round(((scaledValue - rangeMin) * domainValues) / (rangeMax - rangeMin));
    }


    public override string GetDomainValueName(double domainValue)
    {
        //Covert domainValue to int
        int domainValueToInt = Convert.ToInt32(domainValue);
        return scaledValueNames[domainValueToInt];
    }
    
}
