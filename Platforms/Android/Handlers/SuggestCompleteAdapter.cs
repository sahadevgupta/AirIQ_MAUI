using AirIQ.Constants;
using Android.Content;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Microsoft.Maui.Platform;

namespace AirIQ.Platforms.Android.Handlers
{
    internal class SuggestCompleteAdapter : ArrayAdapter, IFilterable
    {
        readonly SuggestFilter filter = new SuggestFilter();
        private List<object> resultList;
        Func<object, string> labelFunc;

        private string _displayMemberPath;

        public SuggestCompleteAdapter(Context context, int resource, int textViewResourceId, string displayMemberPath) : base(context, resource, textViewResourceId)
        {
            resultList = new List<object>();
            SetNotifyOnChange(true);

            _displayMemberPath = displayMemberPath;
        }

        public void UpdateList(IEnumerable<object> list, Func<object, string> labelFunc)
        {
            this.labelFunc = labelFunc;
            resultList = list.ToList();

            filter.SetFilter(resultList.Select(x => labelFunc(x)));
            NotifyDataSetChanged();
        }

        public override int Count => resultList.Count;

        public override global::Android.Views.View GetView(int position, global::Android.Views.View? convertView, ViewGroup parent)
        {
            var view = base.GetView(position, convertView, parent);
            var textview = view.FindViewById<TextView>(Resource.Id.autocomplete_textview);
            var text = textview.Text;
            if (text.Contains(StringConstants.NoResultAvailable, StringComparison.OrdinalIgnoreCase))
            {
                textview.SetTextColor(Colors.Red.ToPlatform());
            }
            return view;
        }

        public override Filter Filter => filter;

        public override Java.Lang.Object? GetItem(int position)
        {
            return labelFunc(GetObject(position));
        }

        private object GetObject(int position)
        {
            return resultList[position];
        }
    }

    internal class SuggestFilter : Filter
    {
        IEnumerable<string> resultList;
        public void SetFilter(IEnumerable<string> list)
        {
            this.resultList = list;
        }
        protected override FilterResults? PerformFiltering(ICharSequence? constraint)
        {
            if (resultList == null)
            {
                return new FilterResults() { Count = 0, Values = null };
            }

            return new FilterResults { Count = resultList.Count(), Values = resultList.ToArray() };
        }

        protected override void PublishResults(ICharSequence? constraint, FilterResults? results)
        {

        }
    }
}
