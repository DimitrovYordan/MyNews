import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';

import { Scene, PerspectiveCamera, WebGLRenderer, SphereGeometry, 
    MeshStandardMaterial, Mesh, TextureLoader, AmbientLight, DirectionalLight } from 'three';

import { LoadingService } from '../../services/loading.service';

@Component({
    selector: 'app-loader',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './loader.component.html',
    styleUrls: ['./loader.component.scss']
})
export class LoaderComponent implements OnInit, OnDestroy {
    @ViewChild('globeCanvas', { static: true }) globeCanvas!: ElementRef<HTMLDivElement>;

    private scene!: Scene;
    private camera!: PerspectiveCamera;
    private renderer!: WebGLRenderer;
    private sphere!: Mesh;
    private animationId!: number;    

    constructor(private loadingService: LoadingService) { }

    ngOnInit(): void {
        this.initThree();
        this.animate();
    }

    ngOnDestroy(): void {
        cancelAnimationFrame(this.animationId);
        if (this.renderer) this.renderer.dispose();
    }

    get isLoading() {
        return this.loadingService.isLoading();
    }

    private initThree() {
        const container = this.globeCanvas.nativeElement;

        const width = 100;
        const height = 100;

        this.scene = new Scene();
        this.camera = new PerspectiveCamera(75, width / height, 0.1, 1000);
        this.camera.position.z = 2.5;

        this.renderer = new WebGLRenderer({ antialias: true, alpha: true });
        this.renderer.setSize(width, height);
        container.appendChild(this.renderer.domElement);

        const geometry = new SphereGeometry(1, 32, 32);
        const loader = new TextureLoader();

        loader.load('/assets/land_shallow_topo_2048.jpg', (texture) => {
            const material = new MeshStandardMaterial({
                map: texture
            });
            this.sphere = new Mesh(geometry, material);
            this.scene.add(this.sphere);
        });

        const ambientLight = new AmbientLight(0xffffff, 0.6);
        this.scene.add(ambientLight);
        const directionalLight = new DirectionalLight(0xffffff, 0.5);
        directionalLight.position.set(3, 2, 3);
        this.scene.add(directionalLight);
    }

    private animate = () => {
        this.animationId = requestAnimationFrame(this.animate);
        if (this.sphere) {
            this.sphere.rotation.y += 0.02;
        }
        this.renderer.render(this.scene, this.camera);
    }
}
