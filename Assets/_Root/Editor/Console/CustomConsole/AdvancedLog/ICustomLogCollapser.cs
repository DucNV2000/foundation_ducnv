﻿using System.Collections.Generic;
using UnityEditor;

namespace Pancake.Console
{
	internal interface ICustomLogCollapser
	{
		// int GetCount(int index);
		bool OnHandleLog(LogEntry entry, int row, string preview, List<CachedConsoleInfo> entries);
	}
}