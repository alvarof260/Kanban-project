
export interface Task {
  id: number;
  nombre: string;
  estado: number;
  descripcion: string;
  color: string;
  idUsuarioAsignado: number;
  nombreUsuarioAsignado: string;
}
