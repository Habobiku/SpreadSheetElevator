namespace Spreadsheet.ComputeSheets.Funcition
{
	public static class InitFunctions
	{
		public static List<Function> AllFunctionsInit()
		{
			return new List<Function> {new AssignValue(), new SumFunction(), new MultiplyFunction(), new DivideFunction(), new GtFunction(), new EqFunction(), new NotFunction(), new AndFunction(), new OrFunction(), new IfFunction(), new ConcatFunction(), /*new LtFunction()*/ };
		}
        public static List<Function> BoolFunctionsInit()
        {
            return new List<Function> {new GtFunction(), new EqFunction(), new NotFunction(), new AndFunction(), new OrFunction()};
        }
    }
}

