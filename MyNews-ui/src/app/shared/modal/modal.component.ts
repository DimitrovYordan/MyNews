import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './modal.component.html',
  styleUrl: './modal.component.scss'
})
export class ModalComponent {
  @Input() show: boolean = false;
  @Input() type: 'success' | 'error' = 'success';
  @Input() message: string = '';

  @Output() closed = new EventEmitter<void>();

  close() {
    this.closed.emit();
  }
}
