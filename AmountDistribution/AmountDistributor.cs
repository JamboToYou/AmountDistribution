namespace AmountDistribution
{
	public static class AmountDistributor
	{
		public static string Distribute(DistributionTypes type, decimal amount, string sumsStr)
		{
			var sums = sumsStr
				.Split(';')
				.Select(sumStr => decimal.Parse(sumStr))
				.ToArray();

			switch (type)
			{
				case DistributionTypes.PROP:
					return string.Join(';', DistributePROP(amount, sums));
				case DistributionTypes.ASC:
					return string.Join(';', DistributeASC(amount, sums));
				case DistributionTypes.DESC:
					return string.Join(';', DistributeDESC(amount, sums));
				default:
					return sumsStr;
			}
		}

		private static decimal[] DistributePROP(decimal amount, decimal[] sums)
		{
			var total = sums.Sum();

			if (total == 0)
				return sums;

			var koef = amount / total;

			for (int i = 0; i < sums.Length; i++)
			{
				var exactValue = sums[i] * koef;
				var sum = Math.Round(exactValue, 2);

				sums[sums.Length - 1] += exactValue - sum;

				sums[i] = sum;
			}

			return sums;
		}

		private static decimal[] DistributeASC(decimal amount, decimal[] sums)
		{
			for (int i = 0; i < sums.Length; i++)
			{
				if (amount == 0)
				{
					sums[i] = 0;
				}
				else if (amount < sums[i])
				{
					sums[i] = amount;
					amount = 0;
				}
				else
				{
					amount -= sums[i];
				}
			}

			return sums;
		}

		private static decimal[] DistributeDESC(decimal amount, decimal[] sums)
		{
			for (int i = sums.Length - 1; i >= 0; i--)
			{
				if (amount == 0)
				{
					sums[i] = 0;
				}
				else if (amount < sums[i])
				{
					sums[i] = amount;
					amount = 0;
				}
				else
				{
					amount -= sums[i];
				}
			}

			return sums;
		}
	}
}
