import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { CoffeeComponent } from './components/coffee/coffee.component';
import { AdminComponent } from './components/admin/admin.component';
import { Api } from './services/api';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        AdminComponent,
        CoffeeComponent
    ],
    providers: [Api],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'coffee', pathMatch: 'full' },
            { path: 'admin', component: AdminComponent },
            { path: 'coffee', component: CoffeeComponent },
            { path: '**', redirectTo: 'coffee' }
        ])
    ]
})
export class AppModuleShared {
}
