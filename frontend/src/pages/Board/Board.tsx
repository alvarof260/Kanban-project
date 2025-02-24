import { useEffect, useState } from "react";
import { useParams } from "react-router";
import { CustomModal } from "../../components";
import { ESTADOS } from "../../constants";
import { Task } from "../../models";
import { ColumnTask, TitleColumn, GroupTask, TaskCard, FormTask, FormTaskCreate, TaskUpdateValues } from "./components";
import { AssignTaskForm, AssignTaskValues } from "./components/AssignTaskForm/AssignTaskForm";

export const Board = () => {
  const { idBoard } = useParams();
  const [tasks, setTasks] = useState<Task[]>([]);
  const [isOpen, setIsOpen] = useState<"create" | "edit" | "assign" | "delete" | "none">("none");
  const [infoActions, setInfoActions] = useState<{ estado: number, id: number, idUsuarioAsignado: number }>({
    estado: 0,
    id: 0,
    idUsuarioAsignado: 0
  });

  useEffect(() => {
    const fetchData = async () => {
      const options: RequestInit = {
        method: "GET",
        headers: { "Content-Type": "application/json" },
        credentials: "include"
      };

      try {
        const response = await fetch(`http://localhost:5093/api/Tarea/Tablero/${idBoard}`, options);

        if (!response.ok) {
          throw new Error('Error al conectar con el servidor.');
        }

        const data: { success: boolean, data: Task[] } = await response.json();

        if (!data.success) {
          return;
        }

        console.log(data.data);

        setTasks(data.data);
      } catch (err) {
        console.error(err);
      }
    };
    fetchData();
  }, [idBoard]);

  console.log(tasks);

  const handleDeleteTask = async (id: number) => {
    const options: RequestInit = {
      method: "DELETE",
      credentials: "include"
    };

    try {
      const response = await fetch(`http://localhost:5093/api/Tarea/${id}`, options);

      if (!response.ok) {
        throw new Error('Error al conectar con el servidor.');
      }

      const newTasks = tasks.filter((task) => task.id !== id);

      setTasks(newTasks);
    } catch (err) {
      console.error(err);
    }
  };

  const handleUpdateTask = (newState: TaskUpdateValues, id: number) => {
    setTasks(tasks.map((task) => {
      if (task.id === id) {
        return {
          ...task,
          nombre: newState.nombre !== "" ? newState.nombre : task.nombre,
          descripcion: newState.descripcion !== "" ? newState.descripcion : task.descripcion,
          color: newState.color !== "" ? newState.color : task.color,
          estado: newState.estado !== task.estado ? newState.estado : task.estado
        };
      } else {
        return task;
      }
    }));
    setIsOpen("none");
  };

  const handleOpenModalCreate = (state: number) => {
    setIsOpen("create");
    const newState = { ...infoActions, estado: state };
    setInfoActions(newState);
  };

  const handleCreateTask = (newTask: Task) => {
    setTasks([...tasks, newTask]);
  };

  const handleAssignTask = (newState: AssignTaskValues, id: number) => {
    const newTasks = tasks.map((task) =>
      task.id === id
        ? { ...task, idUsuarioAsignado: newState.idUsuarioAsignado }
        : task);
    setTasks(newTasks);
    setIsOpen("none");
  };

  return (
    <>
      <main className="w-screen h-screen bg-gray-100 p-10">
        <section className="flex flex-row w-full h-full">
          {Object.entries(ESTADOS).map(([estado, nombre]) => (
            <ColumnTask key={estado}>
              <TitleColumn name={nombre} />
              <GroupTask onOpenModal={() => handleOpenModalCreate(Number(estado))}>
                {tasks
                  .filter((task) => task.estado === Number(estado))
                  .map((task) => (
                    <TaskCard
                      key={task.id}
                      nombre={task.nombre}
                      id={task.id}
                      state={task.estado}
                      color={task.color}
                      idUsuarioAsignado={task.idUsuarioAsignado}
                      onDeleteTask={handleDeleteTask}
                      onChangeState={(state: number, id: number) => {
                        setIsOpen("edit");
                        const newState = { ...infoActions, id, estado: state };
                        setInfoActions(newState);
                      }}
                      onAssignTask={(id: number, idUsuarioAsignado: number) => {
                        setIsOpen("assign");
                        const newState = { ...infoActions, idUsuarioAsignado, id };
                        setInfoActions(newState);
                      }}
                    />
                  ))}
              </GroupTask>
            </ColumnTask>
          ))}
        </section>
      </main>
      {
        isOpen === "edit" &&
        <CustomModal onModal={() => setIsOpen("none")}>
          <FormTask stateInitial={infoActions.estado} idTask={infoActions.id} onUpdateTask={handleUpdateTask} />
        </CustomModal>
      }
      {
        isOpen === "create" &&
        <CustomModal onModal={() => setIsOpen("none")}>
          <FormTaskCreate state={infoActions.estado} onAddTask={handleCreateTask} />
        </CustomModal>
      }
      {
        isOpen === "assign" &&
        <CustomModal onModal={() => setIsOpen("none")}>
          <AssignTaskForm idUsuarioAsignado={infoActions.idUsuarioAsignado} idTask={infoActions.id} onAssignTask={handleAssignTask} />
        </CustomModal>
      }
    </>
  );
};
