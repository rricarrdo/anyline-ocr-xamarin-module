﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Android.Util;
using AT.Nineyards.Anyline.Models;
using IO.Anyline.Plugin;

namespace AnylineExamples.Droid
{
    [Activity(Label = "",
        MainLauncher = false,
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait,
        HardwareAccelerated = true)]
    public class ResultActivity : AppCompatActivity
    {
        public static readonly string TAG = typeof(ResultActivity).Name;

        private ListView _resultListView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            SetContentView(Resource.Layout.result_activity);
            _resultListView = FindViewById<ListView>(Resource.Id.result_list_view);
            
            var handle = new IntPtr(Intent.GetIntExtra("handle", 0));
            var scanResult = GetObject<ScanResult>(handle, JniHandleOwnership.DoNotTransfer);
            
            if (scanResult != null)
            {
                Title = scanResult.GetType().Name;

                var list = new List<Java.Lang.Object>();
                foreach (var prop in scanResult.GetType().GetProperties())
                {

                    switch (prop.Name)
                    {
                        // filter out properties that we don't want to display
                        case "JniPeerMembers":
                        case "JniIdentityHashCode":
                        case "Handle":
                        case "PeerReference":
                        case "Outline":
                            break;
                        default:

                            var value = prop.GetValue(scanResult, null);

                            Log.Debug(TAG, "{0}: {1}", prop.Name, value);
                            if (value != null)
                            {
                                if (value.GetType().Equals(typeof(AnylineImage)))
                                {
                                    var bitmap = (value as AnylineImage).Clone().Bitmap;
                                    list.Add(prop.Name);
                                    list.Add(bitmap);
                                }
                                else
                                {
                                    list.Add(prop.Name);
                                    list.Add(value.ToString());
                                }
                            }
                            break;
                    }

                }
                
                var listAdapter = new ResultListAdapter(this, list);
                _resultListView.Adapter = listAdapter;
                Util.SetListViewHeightBasedOnChildren(_resultListView, this);
            }
        }

        #region going back & cleanup
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    GoBack();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnBackPressed()
        {
            GoBack();
        }

        private void GoBack()
        {
            Finish();
        }
        #endregion


    }
}