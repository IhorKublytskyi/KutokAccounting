using Microsoft.Extensions.Localization;
using MudBlazor;

namespace KutokAccounting;

public class UkrainianLocalizer : MudLocalizer
{
	private readonly Dictionary<string, string> _localization;

	public override LocalizedString this[string key]
	{
		get
		{
			if (_localization.TryGetValue(key, out string? result))
			{
				return new LocalizedString(key, result);
			}

			return new LocalizedString(key, key, true);
		}
	}

	public UkrainianLocalizer()
	{
		_localization = new Dictionary<string, string>
		{
			{
				"Converter_ConversionError", "Помилка перетворення: {0}"
			},
			{
				"Converter_ConversionFailed", "Не вдалося перетворити з {0} у {1}: {2}"
			},
			{
				"Converter_ConversionNotImplemented", "Перетворення у тип {0} не реалізовано"
			},
			{
				"Converter_InvalidBoolean", "Недійсне булеве значення"
			},
			{
				"Converter_InvalidDateTime", "Недійсна дата або час"
			},
			{
				"Converter_InvalidGUID", "Недійсний GUID"
			},
			{
				"Converter_InvalidNumber", "Недійсне число"
			},
			{
				"Converter_InvalidTimeSpan", "Недійсний проміжок часу"
			},
			{
				"Converter_InvalidType", "Недійсний тип {0}"
			},
			{
				"Converter_NotValueOf", "Не є значенням {0}"
			},
			{
				"Converter_UnableToConvert", "Неможливо перетворити у {0} з типу {1}"
			},
			{
				"HeatMap_Less", "Менше"
			},
			{
				"HeatMap_More", "Більше"
			},
			{
				"MudAlert_Close", "Закрити"
			},
			{
				"MudBaseDatePicker_NextMonth", "Наступний місяць {0}"
			},
			{
				"MudBaseDatePicker_NextYear", "Наступний рік {0}"
			},
			{
				"MudBaseDatePicker_Open", "Відкрити"
			},
			{
				"MudBaseDatePicker_PrevMonth", "Попередній місяць {0}"
			},
			{
				"MudBaseDatePicker_PrevYear", "Попередній рік {0}"
			},
			{
				"MudCarousel_Index", "Індекс {0}"
			},
			{
				"MudCarousel_Next", "Наступний"
			},
			{
				"MudCarousel_Previous", "Попередній"
			},
			{
				"MudChip_Close", "Закрити"
			},
			{
				"MudColorPicker_AlphaSlider", "Повзунок прозорості"
			},
			{
				"MudColorPicker_Close", "Закрити"
			},
			{
				"MudColorPicker_GridView", "Сітка"
			},
			{
				"MudColorPicker_HideSwatches", "Приховати палітру"
			},
			{
				"MudColorPicker_HueSlider", "Повзунок відтінку"
			},
			{
				"MudColorPicker_ModeSwitch", "Змінити режим"
			},
			{
				"MudColorPicker_Open", "Відкрити"
			},
			{
				"MudColorPicker_PaletteView", "Палітра"
			},
			{
				"MudColorPicker_ShowSwatches", "Показати палітру"
			},
			{
				"MudColorPicker_SpectrumView", "Спектр"
			},
			{
				"MudDataGrid_AddFilter", "Додати фільтр"
			},
			{
				"MudDataGrid_Apply", "Застосувати"
			},
			{
				"MudDataGrid_Cancel", "Скасувати"
			},
			{
				"MudDataGrid_Clear", "Очистити"
			},
			{
				"MudDataGrid_ClearFilter", "Очистити фільтр"
			},
			{
				"MudDataGrid_CollapseAllGroups", "Згорнути всі групи"
			},
			{
				"MudDataGrid_Column", "Стовпець"
			},
			{
				"MudDataGrid_Columns", "Стовпці"
			},
			{
				"MudDataGrid_Contains", "містить"
			},
			{
				"MudDataGrid_EndsWith", "закінчується на"
			},
			{
				"MudDataGrid_Equals", "дорівнює"
			},
			{
				"MudDataGrid_EqualSign", "="
			},
			{
				"MudDataGrid_ExpandAllGroups", "Розгорнути всі групи"
			},
			{
				"MudDataGrid_False", "хибно"
			},
			{
				"MudDataGrid_Filter", "Фільтрувати"
			},
			{
				"MudDataGrid_FilterValue", "Значення фільтра"
			},
			{
				"MudDataGrid_GreaterThanOrEqualSign", "≥"
			},
			{
				"MudDataGrid_GreaterThanSign", ">"
			},
			{
				"MudDataGrid_Group", "Група"
			},
			{
				"MudDataGrid_Hide", "Приховати"
			},
			{
				"MudDataGrid_HideAll", "Приховати все"
			},
			{
				"MudDataGrid_Is", "є"
			},
			{
				"MudDataGrid_IsAfter", "після"
			},
			{
				"MudDataGrid_IsBefore", "перед"
			},
			{
				"MudDataGrid_IsEmpty", "порожнє"
			},
			{
				"MudDataGrid_IsNot", "не є"
			},
			{
				"MudDataGrid_IsNotEmpty", "не порожнє"
			},
			{
				"MudDataGrid_IsOnOrAfter", "на або після"
			},
			{
				"MudDataGrid_IsOnOrBefore", "на або перед"
			},
			{
				"MudDataGrid_LessThanOrEqualSign", "≤"
			},
			{
				"MudDataGrid_LessThanSign", "<"
			},
			{
				"MudDataGrid_Loading", "Завантаження..."
			},
			{
				"MudDataGrid_MoveDown", "Перемістити вниз"
			},
			{
				"MudDataGrid_MoveUp", "Перемістити вгору"
			},
			{
				"MudDataGrid_NotContains", "не містить"
			},
			{
				"MudDataGrid_NotEquals", "не дорівнює"
			},
			{
				"MudDataGrid_NotEqualSign", "≠"
			},
			{
				"MudDataGrid_OpenFilters", "Відкрити фільтри"
			},
			{
				"MudDataGrid_Operator", "Оператор"
			},
			{
				"MudDataGrid_RefreshData", "Оновити дані"
			},
			{
				"MudDataGrid_RemoveFilter", "Видалити фільтр"
			},
			{
				"MudDataGrid_Save", "Зберегти"
			},
			{
				"MudDataGrid_ShowAll", "Показати все"
			},
			{
				"MudDataGrid_ShowColumnOptions", "Параметри стовпців"
			},
			{
				"MudDataGrid_Sort", "Сортувати"
			},
			{
				"MudDataGrid_StartsWith", "починається з"
			},
			{
				"MudDataGrid_ToggleGroupExpansion", "Перемкнути групу"
			},
			{
				"MudDataGrid_True", "істинно"
			},
			{
				"MudDataGrid_Ungroup", "Розгрупувати"
			},
			{
				"MudDataGrid_Unsort", "Скасувати сортування"
			},
			{
				"MudDataGrid_Value", "Значення"
			},
			{
				"MudDataGridPager_AllItems", "Усе"
			},
			{
				"MudDataGridPager_FirstPage", "Перша сторінка"
			},
			{
				"MudDataGridPager_InfoFormat", "{0}-{1} з {2}"
			},
			{
				"MudDataGridPager_LastPage", "Остання сторінка"
			},
			{
				"MudDataGridPager_NextPage", "Наступна сторінка"
			},
			{
				"MudDataGridPager_PreviousPage", "Попередня сторінка"
			},
			{
				"MudDataGridPager_RowsPerPage", "Записів на сторінці:"
			},
			{
				"MudDialog_Close", "Закрити"
			},
			{
				"MudFileUpload_FileSizeError", "Файл '{0}' перевищує максимально допустимий розмір {1} байт."
			},
			{
				"MudInput_Clear", "Очистити"
			},
			{
				"MudInput_Decrement", "Зменшити"
			},
			{
				"MudInput_Increment", "Збільшити"
			},
			{
				"MudNavGroup_ToggleExpand", "Перемкнути {0}"
			},
			{
				"MudPageContentNavigation_NavMenu", "Зміст"
			},
			{
				"MudPagination_CurrentPage", "Поточна сторінка {0}"
			},
			{
				"MudPagination_FirstPage", "Перша сторінка"
			},
			{
				"MudPagination_LastPage", "Остання сторінка"
			},
			{
				"MudPagination_NextPage", "Наступна сторінка"
			},
			{
				"MudPagination_PageIndex", "Сторінка {0}"
			},
			{
				"MudPagination_PreviousPage", "Попередня сторінка"
			},
			{
				"MudRatingItem_Label", "Оцінка {0}"
			},
			{
				"MudSnackbar_Close", "Закрити"
			},
			{
				"MudStepper_Complete", "Завершити"
			},
			{
				"MudStepper_Next", "Далі"
			},
			{
				"MudStepper_Previous", "Назад"
			},
			{
				"MudStepper_Reset", "Скинути"
			},
			{
				"MudStepper_Skip", "Пропустити"
			},
			{
				"MudTablePager_FirstPage", "Перша сторінка"
			},
			{
				"MudTablePager_LastPage", "Остання сторінка"
			},
			{
				"MudTablePager_NextPage", "Наступна сторінка"
			},
			{
				"MudTablePager_PreviousPage", "Попередня сторінка"
			},
			{
				"MudTimePicker_Open", "Відкрити"
			},
			{
				"MudTreeView_CollapseItem", "Згорнути"
			},
			{
				"MudTreeView_ExpandItem", "Розгорнути"
			}
		};
	}
}