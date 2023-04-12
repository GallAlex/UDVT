using System.Collections.Generic;
using UnityEngine;

public class ScaleLinear : Scale
{

    private double domainMin = 0.0d;
    private double domainMax = 100.0d;

    private double rangeMin = 0.0d;
    private double rangeMax = 1.0d;


    public ScaleLinear(List<double> domain) : base(domain)
    {
        dataScaleType = DataScaleType.Linear;
        
        domainMin = domain[0];
        domainMax = domain[1];

        rangeMin = range[0];
        rangeMax = range[1];
    }

    public ScaleLinear(List<double> domain, List<double> range) : base(domain, range)
    {
        dataScaleType = DataScaleType.Linear;
        
        domainMin = domain[0];
        domainMax = domain[1];

        rangeMin = range[0];
        rangeMax = range[1];
    }

    public override double GetScaledValue(double domainValue)
    {
        var domainRange = domainMax - domainMin;
        var newRange = rangeMax - rangeMin;

        if (domainRange == 0.0d || newRange == 0.0d)
        {
            Debug.LogWarning("Min/Max of domain or range are equal!");
            return 0.0d;
        }

        return (((domainValue - domainMin) * newRange) / domainRange) + rangeMin;
    }

    public override double GetDomainValue(double scaledValue)
    {
        var scaledRange = rangeMax - rangeMin;
        var newDomain = domainMax - domainMin;

        if (scaledRange == 0.0d || newDomain == 0.0d)
        {
            Debug.LogWarning("Min/Max of domain or range are equal!");
            return 0.0d;
        }

        return (((scaledValue - rangeMin) * newDomain) / scaledRange) + domainMin;
    }

    public override string GetDomainValueName(double domainValue)
    {
        return domainValue.ToString();
    }

    /// <summary>
    /// Returns a array normalized to the given range from its domain values
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public double[] GetNormalizedArray(double[] array)
    {
        double[] normalizedArray = new double[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            normalizedArray[i] = GetScaledValue((double)array[i]);
        }

        return normalizedArray;
    }

}
