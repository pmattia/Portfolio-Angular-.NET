import { animate, animateChild, group, query as q, style, transition, trigger } from '@angular/animations';
const query = (s: any, a: any, o = { optional: true }) => q(s, a, o);

export const routerTransition = trigger('primaryTransition', [
  transition('greeting => conversation', [
    group([
      query(':leave', [
        style({ 
          opacity: 1
        })
      ]),
      query(':leave .stripe-bg', [
        style({
          top: '-100px'
        })
      ]),
      query(':enter', [
        style({
          opacity: 0,
          position: 'absolute',
          top: 0,
          left: 0,
          width: '100vw',
          height: '100vh'
        })
      ])
    ]),
    query(':leave', animateChild()),
    group([
      query(':leave .stripe-bg', [
        animate('800ms ease-out',
          style({
            top: '-1000px'
          }))
      ]),
      query(':leave', [
        animate('500ms 500ms ease-out', style({ opacity: 0 }))
      ]),
      query(':enter', [
        animate('500ms 1000ms ease-out', style({ opacity: 1 }))
      ]),
      query('@*', animateChild())
    ]),
  ]),

  transition('* => greeting', [
    group([
      query(':enter', [
        style({ opacity: 0 })
      ]),
      query(':enter .stripe-bg', [
        style({
          top: '-1000px'
        })
      ]),
      query(':leave', [
        style({
          opacity: 0,
          position: 'absolute',
          top: 0,
          left: 0,
          width: '100vw',
          height: '100vh'
        })
      ])
    ]),
    query(':leave', animateChild()),
    group([
      query(':enter .stripe-bg', [
        animate('800ms ease-out',
          style({
            top: '-100px'
          }))
      ]),
      query(':enter', [
        animate('500ms 500ms ease-out', style({ opacity: 1 }))
      ]),
      query(':leave', [
        animate('500ms 1000ms ease-out', style({ opacity: 0 }))
      ]),
      query('@*', animateChild())
    ]),
  ])
]);
