import { Routes } from '@angular/router';

export const routes: Routes = [
	{ path: '', redirectTo: 'feed', pathMatch: 'full' },
	{
		path: 'feed',
		loadComponent: () => import('./pages/feed/feed').then((m) => m.Feed),
	},
];
