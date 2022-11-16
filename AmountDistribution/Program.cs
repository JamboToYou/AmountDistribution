using AmountDistribution;

// expected: 416.67;833.33;1250;2083.33;3333.33;2083.34
Console.WriteLine(AmountDistributor.Distribute(DistributionTypes.PROP, 10000, "1000;2000;3000;5000;8000;5000"));

// expected: 1000;2000;3000;4000;0;0
Console.WriteLine(AmountDistributor.Distribute(DistributionTypes.ASC, 10000, "1000;2000;3000;5000;8000;5000"));

// expected: 0;0;0;0;5000;5000
Console.WriteLine(AmountDistributor.Distribute(DistributionTypes.DESC, 10000, "1000;2000;3000;5000;8000;5000"));

try
{
	Console.WriteLine(AmountDistributor.Distribute(DistributionTypes.DESC, -10000, "1000;2000;3000;5000;8000;5000"));
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
}

try
{
	Console.WriteLine(AmountDistributor.Distribute(DistributionTypes.DESC, 10000, "-1000;2000;3000;5000;8000;5000"));
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
}