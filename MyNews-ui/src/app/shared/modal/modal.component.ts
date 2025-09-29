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
  @Input() showCancel: boolean = true;

  @Output() closed = new EventEmitter<boolean>();

  confirm() {
    this.closed.emit(true);
  }

  cancel() {
    this.closed.emit(false);
    this.show = false;
  }

  close() {
    this.closed.emit();
  }
}
