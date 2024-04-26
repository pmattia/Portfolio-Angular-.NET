import { Component, HostListener } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { ChildrenOutletContexts, NavigationEnd, Router } from "@angular/router";
import { Store, select } from "@ngrx/store";
import { DeviceDetectorService } from "ngx-device-detector";
import { MenuItem } from "primeng/api/menuitem";
import { Observable, filter, tap } from "rxjs";
import { routerTransition } from "./core/animations/primary-outlet.animation";
import { curriculumVitae } from "./core/contants";
import { AppState } from "./core/store/models/app-state.model";
import { uiSelector, userConsent } from "./core/store/selectors";
import { UiState } from "./core/store/models/ui-state.model";
import { contactMe, goHome, setMobile, showPdf } from "./core/store/actions/ui.actions";
import { SubscriptionDestroyer } from "projects/gnappo-lib";


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  animations: [routerTransition]
})


export class AppComponent extends SubscriptionDestroyer {

  title = $localize`Pietro Mattia Guglielmo's Portfolio`;
  resetLabel = $localize`Restart`;
  resumeLabel = $localize`My Resume`;
  contactLabel = $localize`Contact me`;
  items: MenuItem[];
  ui$: Observable<UiState>;
  userConsent$: Observable<boolean>;
  private userHasInteracted = false;
  
  constructor(private store: Store<AppState>,
    title: Title,
    private contexts: ChildrenOutletContexts,
    router: Router,
    private deviceService: DeviceDetectorService) 
    {
    super();
    title.setTitle(this.title);
    this.ui$ = this.store.pipe(select(uiSelector));
    this.userConsent$ = this.store.pipe(select(userConsent));  
   
    //set ismobile
    this.store.dispatch(setMobile({ isMobile: this.deviceService.isMobile() }));

    //init menu
    this.items = [{
      label: this.resetLabel,
      command: () => {
        this.store.dispatch(goHome());
      }
    },
    {
      label: this.resumeLabel,
      command: () => {
        this.store.dispatch(showPdf({ pdfName: curriculumVitae }));
      }
    },
    {
      label: this.contactLabel,
      command: () => {
        this.store.dispatch(contactMe());
      }
    }];
  }

  @HostListener('click', ['$event'])
  onClick(event: Event) {
    this.userHasInteracted = true;
  }

  @HostListener('window:beforeunload', ['$event'])
  onWindowBeforeUnload(event: Event) {
    if (this.userHasInteracted) {
      //show browser confirmation dialog
      event.preventDefault();
    }
  }

  getRouteAnimationData() {
    const animation = this.contexts.getContext('primary')?.route?.snapshot?.data?.['animation'];
    return animation;
  }
}
