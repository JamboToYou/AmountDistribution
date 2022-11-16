namespace AmountDistribution
{
	public static class AmountDistributor
	{
		/// <summary>
		/// Функция для распределения суммы
		/// </summary>
		/// <param name="type">Тип распределения суммы</param>
		/// <param name="amount">Распределяемая сумма</param>
		/// <param name="sumsStr">Строка для сумм через ";", по которым будет распределение</param>
		/// <returns>Строка сумм через ";" с распределенными суммами</returns>
		/// <exception cref="ArgumentNullException">Выбрасывается, когда <paramref name="sumsStr"/> равна NULL</exception>
		/// <exception cref="FormatException">
		/// Выбрасывается, когда какое-то значение в <paramref name="sumsStr"/> не является числом
		/// </exception>
		/// <exception cref="OverflowException">
		/// Выбрасывается, когда какое-либо значение суммы из <paramref name="sumsStr"/> больше, чем может вместить в себя decimal
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Выбрасывается, когда в <paramref name="sumsStr"/> или в <paramref name="amount"/> содержится отрицательное значение
		/// </exception>
		public static string Distribute(DistributionTypes type, decimal amount, string sumsStr)
		{
			if (amount < 0)
				throw new ArgumentException("Распределение отрицательной суммы не разрешено");

			// Преобразование строки сумм в массив числового типа
			var sums = sumsStr
				.Split(';')
				.Select(sumStr => decimal.Parse(sumStr))
				.ToArray();

			if (sums.Any(sum => sum < 0))
				throw new ArgumentException("Распределение на отрицательные суммы не разрешено");

			switch (type)
			{
				case DistributionTypes.PROP: // если тип рапределения "пропорционально"
					return string.Join(';', DistributePROP(amount, sums));
				case DistributionTypes.ASC: // если тип рапределения "в счет первых"
					return string.Join(';', DistributeASC(amount, sums));
				case DistributionTypes.DESC: // если тип рапределения "в счет последних"
					return string.Join(';', DistributeDESC(amount, sums));
				default:
					return sumsStr;
			}
		}

		/// <summary>
		/// Функция для распределения сумм типа "пропорционально"
		/// </summary>
		/// <param name="amount">Распределяемая сумма</param>
		/// <param name="sums">Массив сумм, по которым будет распределение</param>
		/// <returns>Массив распределенных сумм</returns>
		private static decimal[] DistributePROP(decimal amount, decimal[] sums)
		{
			// Считаем общую сумму всех сумм из массива
			var total = sums.Sum();

			// Возвращаем исходный массив, если сумма элементов равна 0
			if (total == 0)
				return sums;

			// Вычисляем коэффициент на рубль для сумм из массива
			var koef = amount / total;

			for (int i = 0; i < sums.Length; i++)
			{
				// Точное значение распределения для текущей суммы
				var exactValue = sums[i] * koef;

				// Значение распределения, округленное до копеек
				var sum = Math.Round(exactValue, 2);

				// Добавляем остаток от округления к последней сумме в массиве
				sums[sums.Length - 1] += exactValue - sum;

				// Сохраняем окруленное до копеек значение вместо текущей суммы
				sums[i] = sum;
			}

			return sums;
		}

		/// <summary>
		/// Функция для распределения сумм типа "в счет первых"
		/// </summary>
		/// <param name="amount">Распределяемая сумма</param>
		/// <param name="sums">Массив сумм, по которым будет распределение</param>
		/// <returns>Массив распределенных сумм</returns>
		private static decimal[] DistributeASC(decimal amount, decimal[] sums)
		{
			// Перебираем массив сумм от начала
			for (int i = 0; i < sums.Length; i++)
			{
				// Если в распределяемой сумме нет средств,
				// то назначаем 0 вместо текущей суммы
				if (amount == 0)
				{
					sums[i] = 0;
				}
				// Иначе если распределяемая сумма меньше значения текущей суммы,
				// то назначаем остаток вместо текущей суммы, а распределяемую сумму обнуляем
				else if (amount < sums[i])
				{
					sums[i] = amount;
					amount = 0;
				}
				// Иначе оставляем текущую сумму как есть и уменьшаем рапределяемую сумму на значение текущей суммы
				else
				{
					amount -= sums[i];
				}
			}

			return sums;
		}

		/// <summary>
		/// Функция для распределения сумм типа "в счет последних"
		/// </summary>
		/// <param name="amount">Распределяемая сумма</param>
		/// <param name="sums">Массив сумм, по которым будет распределение</param>
		/// <returns>Массив распределенных сумм</returns>
		private static decimal[] DistributeDESC(decimal amount, decimal[] sums)
		{
			// Перебираем массив сумм от конца
			for (int i = sums.Length - 1; i >= 0; i--)
			{
				// Если в распределяемой сумме нет средств,
				// то назначаем 0 вместо текущей суммы
				if (amount == 0)
				{
					sums[i] = 0;
				}
				// Иначе если распределяемая сумма меньше значения текущей суммы,
				// то назначаем остаток вместо текущей суммы, а распределяемую сумму обнуляем
				else if (amount < sums[i])
				{
					sums[i] = amount;
					amount = 0;
				}
				// Иначе оставляем текущую сумму как есть и уменьшаем рапределяемую сумму на значение текущей суммы
				else
				{
					amount -= sums[i];
				}
			}

			return sums;
		}
	}
}
