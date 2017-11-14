import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { AdminComponent } from './components/admin/admin.component';
import { Api } from './services/api';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        AdminComponent,
        FetchDataComponent
    ],
    providers: [Api],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'fetch-data', pathMatch: 'full' },
            { path: 'admin', component: AdminComponent },
            { path: 'fetch-data', component: FetchDataComponent },
            { path: '**', redirectTo: 'fetch-data' }
        ])
    ]
})
export class AppModuleShared {
}
