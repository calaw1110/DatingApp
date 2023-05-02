import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MerberListComponent } from './members/merber-list/merber-list.component';
import { MerberDetailComponent } from './members/merber-detail/merber-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'members', component: MerberListComponent, canActivate: [AuthGuard] },
    { path: 'members/:id', component: MerberDetailComponent },
    { path: 'lists', component: ListsComponent },
    { path: 'messages', component: MessagesComponent },
    { path: '**', component: HomeComponent, pathMatch: 'full' },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
