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
				//Create Alert
//				var detailAlert = UIAlertController.Create ("Annotation Clicked", "You clicked on " + 
//					(annotation as MKAnnotation).Coordinate.Latitude.ToString() + ", " +
//					(annotation as MKAnnotation).Coordinate.Longitude.ToString(), UIAlertControllerStyle.Alert);
//				detailAlert.AddAction (UIAlertAction.Create ("OK", UIAlertActionStyle.Default, null));
//				parent.PresentViewController (detailAlert, true, null); 
				if (parent.ParentViewController.NavigationController != null) {
					EventDescViewController vc = parent.Storyboard.InstantiateViewController ("EventDescVC") as EventDescViewController;
                    //vc.ObjId = (annotation as BasicMapAnnotation).ObjId;
                    vc.Bonus = Data.GetBonusOffer((annotation as BasicMapAnnotation).ObjId); // ObjId == CafeId
                    parent.ParentViewController.NavigationController.PushViewController (vc, true);
					parent.ParentViewController.NavigationController.SetNavigationBarHidden(false, true);
				}
			};
			annotationView.RightCalloutAccessoryView = detailButton;

			annotationView.LeftCalloutAccessoryView = new UIImageView(UIImage.FromBundle("29_icon.png"));

			return annotationView;
		}

		// as an optimization, you should override this method to add or remove annotations as the 
		// map zooms in or out.
		public override void RegionChanged (MKMapView mapView, bool animated) {}
	}

	partial class MapViewController : UIViewController
	{
		public MapViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			//MapLabel.Text = string.Format ("Map. Image: h{0}, w{1}", ivPlace.Frame.Height, ivPlace.Frame.Width);

			var annotation = new BasicMapAnnotation (new CLLocationCoordinate2D(54.9771215,73.3842507), "Omsk", "North City");
			Map.AddAnnotation(annotation);

			var coords = new CLLocationCoordinate2D(54.9771215,73.3842507);
			var span = new MKCoordinateSpan(MilesToLatitudeDegrees(4), MilesToLongitudeDegrees(4, coords.Latitude));
			Map.Region = new MKCoordinateRegion(coords, span);

			foreach (var cafe in Data.Cafes)
			{
                var coordinate = new CLLocationCoordinate2D(cafe.GeoLocation.GeoPoint.Latitude, cafe.GeoLocation.GeoPoint.Latitude);

                var ann = new BasicMapAnnotation (coordinate, string.Format("Id: {0}", cafe.Id), cafe.FullAddress, cafe.Id);
				Map.AddAnnotation(ann);
				Map.Delegate = new MapDelegate (this);
			}
		}

		public double MilesToLatitudeDegrees(double miles)
		{
			double earthRadius = 3960.0; // in miles
			double radiansToDegrees = 180.0/Math.PI;
			return (miles/earthRadius) * radiansToDegrees;
		}

		public double MilesToLongitudeDegrees(double miles, double atLatitude)
		{
			double earthRadius = 3960.0; // in miles
			double degreesToRadians = Math.PI/180.0;
			double radiansToDegrees = 180.0/Math.PI;
			// derive the earth's radius at that point in latitude
			double radiusAtLatitude = earthRadius * Math.Cos(atLatitude * degreesToRadians);
			return (miles / radiusAtLatitude) * radiansToDegrees;
		}
	}
}
