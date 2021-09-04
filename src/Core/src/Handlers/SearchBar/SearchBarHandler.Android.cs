using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Widget;
using static AndroidX.AppCompat.Widget.SearchView;
using SearchView = AndroidX.AppCompat.Widget.SearchView;

namespace Microsoft.Maui.Handlers
{
	public partial class SearchBarHandler : ViewHandler<ISearchBar, SearchView>
	{
		QueryTextListener QueryListener { get; } = new QueryTextListener();

		static Drawable? DefaultBackground;
		static ColorStateList? DefaultPlaceholderTextColors { get; set; }

		EditText? _editText;
		public EditText? QueryEditor => _editText;

		protected override SearchView CreateNativeView()
		{
			var searchView = new SearchView(Context);
			searchView.SetIconifiedByDefault(false);

			_editText = searchView.GetFirstChildOfType<EditText>();

			return searchView;
		}

		protected override void ConnectHandler(SearchView nativeView)
		{
			QueryListener.Handler = this;

			nativeView.SetOnQueryTextListener(QueryListener);
		}

		protected override void DisconnectHandler(SearchView nativeView)
		{
			nativeView.SetOnQueryTextListener(null);

			QueryListener.Handler = null;
		}

		void SetupDefaults(SearchView nativeView)
		{
			DefaultBackground = nativeView.Background;
		}

		// This is a Android-specific mapping
		public static void MapBackground(SearchBarHandler handler, ISearchBar searchBar)
		{
			handler.NativeView?.UpdateBackground(searchBar, DefaultBackground);
		}

		public static void MapText(SearchBarHandler handler, ISearchBar searchBar)
		{
			handler.NativeView?.UpdateText(searchBar);
		}

		public static void MapPlaceholder(SearchBarHandler handler, ISearchBar searchBar)
		{
			handler.NativeView?.UpdatePlaceholder(searchBar);
		}

		public static void MapPlaceholderColor(SearchBarHandler handler, ISearchBar searchBar)
		{
			handler.NativeView?.UpdatePlaceholderColor(searchBar, DefaultPlaceholderTextColors, handler._editText);
		}

		public static void MapFont(SearchBarHandler handler, ISearchBar searchBar)
		{
			var fontManager = handler.GetRequiredService<IFontManager>();

			handler.NativeView?.UpdateFont(searchBar, fontManager, handler._editText);
		}

		public static void MapHorizontalTextAlignment(SearchBarHandler handler, ISearchBar searchBar)
		{
			handler.QueryEditor?.UpdateHorizontalTextAlignment(searchBar);
		}

		public static void MapCharacterSpacing(SearchBarHandler handler, ISearchBar searchBar)
		{
			handler.QueryEditor?.UpdateCharacterSpacing(searchBar);
		}

		public static void MapTextColor(SearchBarHandler handler, ISearchBar searchBar)
		{
			handler.QueryEditor?.UpdateTextColor(searchBar);
		}

		[MissingMapper]
		public static void MapIsTextPredictionEnabled(IViewHandler handler, ISearchBar searchBar) { }

		public static void MapMaxLength(SearchBarHandler handler, ISearchBar searchBar)
		{
			handler.NativeView?.UpdateMaxLength(searchBar, handler.QueryEditor);
		}

		[MissingMapper]
		public static void MapIsReadOnly(IViewHandler handler, ISearchBar searchBar) { }

		public static void MapCancelButtonColor(SearchBarHandler handler, ISearchBar searchBar)
		{
			handler.NativeView?.UpdateCancelButtonColor(searchBar);
		}

		public class QueryTextListener : Java.Lang.Object, IOnQueryTextListener
		{
			public SearchBarHandler? Handler { get; set; }

			public bool OnQueryTextChange(string newText)
			{
				return true;
			}

			public bool OnQueryTextSubmit(string newText)
			{
				Handler?.VirtualView?.SearchButtonPressed();

				// TODO: Clear focus

				return true;
			}
		}
	}
}