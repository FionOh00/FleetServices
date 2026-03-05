import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { TableModule } from 'primeng/table';
import { GridComponent } from './grid/grid.component';
import { Router } from '@angular/router';
// import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, MatIconModule, MatButtonModule, TableModule, GridComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {

  isComponentVisible = true;

  protected readonly title = signal('FleetServices');

  isDark: boolean = true;

  ngOnInit() {
    // Load saved theme from localStorage (optional)
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme === 'dark') {
      this.isDark = true;
      this.applyDarkMode();
    } else if (savedTheme === 'light') {
      this.isDark = false;
      this.applyLightMode();
    } else {
      // Default to dark mode
      this.isDark = true;
      this.applyDarkMode();
    }
  }

  toggleTheme() {
    this.isDark = !this.isDark;
    
    if (this.isDark) {
      this.applyDarkMode();
      localStorage.setItem('theme', 'dark');
    } else {
      this.applyLightMode();
      localStorage.setItem('theme', 'light');
    }
    window.location.reload();
  }

  private applyDarkMode() {
    document.documentElement.classList.add('dark');
    document.body.classList.add('dark');
    document.body.setAttribute('data-theme', 'dark');
  }

  private applyLightMode() {
    document.documentElement.classList.remove('dark');
    document.body.classList.remove('dark');
    document.body.setAttribute('data-theme', 'light');
  }
}
