import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-features-section',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './features-section.component.html',
  styleUrls: ['./features-section.component.css']
})
export class FeaturesSectionComponent {
  features = [
    {
      icon: '🥋',
      title: 'Expert Instruction',
      description: 'Learn from highly trained instructors with years of experience in traditional martial arts.'
    },
    {
      icon: '👨‍👩‍👧‍👦',
      title: 'All Ages Welcome',
      description: 'Programs designed for kids, teens, and adults. Everyone can find their path in martial arts.'
    },
    {
      icon: '🏆',
      title: 'Competition Ready',
      description: 'Advanced training for those looking to compete at local, regional, and national levels.'
    },
    {
      icon: '💪',
      title: 'Build Confidence',
      description: 'Develop mental strength, self-discipline, and confidence that extends beyond the dojo.'
    },
    {
      icon: '🤝',
      title: 'Community Focus',
      description: 'Join a supportive community that values respect, integrity, and personal growth.'
    },
    {
      icon: '📅',
      title: 'Flexible Schedule',
      description: 'Multiple class times throughout the week to fit your busy lifestyle.'
    }
  ];
}
