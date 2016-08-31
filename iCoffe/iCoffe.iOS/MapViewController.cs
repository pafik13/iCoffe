using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using MapKit;
using CoreLocation;

using RestSharp;
using System.Globalization;

using iCoffe.Shared;
using System.Collections.Generic;

namespace iCoffe.iOS
{
	class BasicMapAnnotation : MKAnnotation{
		CLLocationCoordinate2D coordinate;
		public override CLLocationCoordinate2D Coordinate {get { return coordinate; } }
		string title, subtitle;
		public override string Title { get{ return title; }}
		public override string Subtitle { get{ return subtitle; }}
		int objid;
		public int ObjId { get{ return objid; }}
		public BasicMapAnnotation (CLLocationCoordinate2D coordinate, string title, string subtitle, int objid = -1) {
			this.coordinate = coordinate;
			this.title = title;
			this.subtitle = subtitle;
			this.objid = objid;
		}
	}

	class MapDelegate : MKMapViewDelegate
	{
		public event EventHandler<UserLocationUpdatedEventArgs> UserLocationUpdated = delegate { };

		protected string annotationIdentifier = "BasicAnnotation";
		UIButton detailButton;
		MapViewController parent;

		public MapDelegate(MapViewController parent)
		{
			this.parent = parent;
		}

		/// <summary>
		/// This is very much like the GetCell method on the table delegate
		/// </summary>
		public override MKAnnotationView GetViewForAnnotation (MKMapView mapView, IMKAnnotation annotation)
		{

			if (ThisIsTheCurrentLocation(mapView, annotation))
			{
				return null;
			}

			// try and dequeue the annotation view
			MKAnnotationView annotationView = mapView.DequeueReusableAnnotation(annotationIdentifier);

			// if we couldn't dequeue one, create a new one
			if (annotationView == null)
				annotationView = new MKPinAnnotationView(annotation, annotationIdentifier);
			else // if we did dequeue one for reuse, assign the annotation to it
				annotationView.Annotation = annotation;

			// configure our annotation view properties
			annotationView.CanShowCallout = true;
			(annotationView as MKPinAnnotationView).AnimatesDrop = true;
			(annotationView as MKPinAnnotationView).PinColor = MKPinAnnotationColor.Green;
			annotationView.Selected = true;

			// you can add an accessory view, in this case, we'll add a button on the right, and an image on the left
			detailButton = UIButton.FromType(UIButtonType.DetailDisclosure);

			detailButton.TouchUpInside += (s, e) => { 
				Console.WriteLine ("Clicked");
				if (parent.ParentViewController.NavigationController != null) {
					if (annotation is BasicMapAnnotation) {
						var bonus = Data.GetBonusOffer((annotation as BasicMapAnnotation).ObjId);
						if (bonus == null) return;

						var vc = parent.Storyboard.InstantiateViewController("BonusVC") as BonusViewController;
						vc.Bonus = bonus;
						parent.ParentViewController.NavigationController.PushViewController(vc, true);
						parent.ParentViewController.NavigationController.SetNavigationBarHidden(false, true);
					}
				}
			};
			annotationView.RightCalloutAccessoryView = detailButton;

			annotationView.LeftCalloutAccessoryView = new UIImageView(UIImage.FromBundle("29_icon.png"));

			return annotationView;
		}

		// as an optimization, you should override this method to add or remove annotations as the 
		// map zooms in or out.
		public override void RegionChanged (MKMapView mapView, bool animated) {}

		public override void DidUpdateUserLocation(MKMapView mapView, MKUserLocation userLocation)
		{
			if (mapView.UserLocation != null)
			{
				CLLocationCoordinate2D coords = mapView.UserLocation.Coordinate;
				MKCoordinateSpan span = new MKCoordinateSpan(MilesToLatitudeDegrees(4), MilesToLongitudeDegrees(4, coords.Latitude));
				mapView.Region = new MKCoordinateRegion(coords, span);
				UserLocationUpdated(this, new UserLocationUpdatedEventArgs(userLocation));
			}
		}

		public double MilesToLatitudeDegrees(double miles)
		{
			double earthRadius = 3960.0; // in miles
			double radiansToDegrees = 180.0 / Math.PI;
			return (miles / earthRadius) * radiansToDegrees;
		}

		public double MilesToLongitudeDegrees(double miles, double atLatitude)
		{
			double earthRadius = 3960.0; // in miles
			double degreesToRadians = Math.PI / 180.0;
			double radiansToDegrees = 180.0 / Math.PI;
			// derive the earth's radius at that point in latitude
			double radiusAtLatitude = earthRadius * Math.Cos(atLatitude * degreesToRadians);
			return (miles / radiusAtLatitude) * radiansToDegrees;
		}

		bool ThisIsTheCurrentLocation(MKMapView mapView, IMKAnnotation annotation)
		{
			var userLocationAnnotation = ObjCRuntime.Runtime.GetNSObject(annotation.Handle) as MKUserLocation;
			if (userLocationAnnotation != null)
			{
				return userLocationAnnotation == mapView.UserLocation;
			}

			return false;
		}
	}

	partial class MapViewController : UIViewController
	{
		public event EventHandler<UserLocationUpdatedEventArgs> UserLocationUpdated = delegate { };

		public MapViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			//UpdateAnnotations();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Map.ShowsUserLocation = true;

			var mapDelegate = new MapDelegate(this);
			mapDelegate.UserLocationUpdated += (sender, e) =>
			{
				UserLocationUpdated(this, e);
			};

			Map.Delegate = mapDelegate;


		}

		public void UpdateAnnotations()
		{
			if (Data.Cafes != null)
			{
				Map.RemoveAnnotations();
				foreach (var cafe in Data.Cafes)
				{
					var coordinate = new CLLocationCoordinate2D(cafe.GeoLocation.GeoPoint.Latitude, cafe.GeoLocation.GeoPoint.Longitude);

					var ann = new BasicMapAnnotation(coordinate, string.Format("Id: {0}", cafe.Id), cafe.FullAddress, cafe.Id);
					Map.AddAnnotation(ann);
				}
			}
		}
	}

	public class UserLocationUpdatedEventArgs : EventArgs
	{
		readonly public MKUserLocation UserLocation;

		public UserLocationUpdatedEventArgs(MKUserLocation userLocation)
		{
			UserLocation = userLocation;
		}
	}
}
