import { useState } from "react";
import { useParams } from "react-router";
import { CardBody, CardFooter, CardHeader, CustomModal } from "../../components";
import { ESTADOS } from "../../constants";
import { Task } from "../../models";
import { ColumnTask, TitleColumn, GroupTask, TaskCard, FormTask, FormTaskCreate, TaskUpdateValues, CardActions } from "./components";
import { AssignTaskForm, AssignTaskValues } from "./components/AssignTaskForm/AssignTaskForm";
import { useFetch } from "../../hooks";

export const Board = () => {
  const { idBoard } = useParams();
  const { data: tasks, setData: setTasks } = useFetch<Task>(`http://localhost:5093/api/Tarea/Tablero/${idBoard}`);
  const [isOpen, setIsOpen] = useState<"create" | "edit" | "assign" | "delete" | "none">("none");
  const [infoActions, setInfoActions] = useState<{ estado: number, id: number, idUsuarioAsignado: number }>({
    estado: 0,
    id: 0,
    idUsuarioAsignado: 0
  });

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
      <main className="w-screen h-screen bg-background-primary p-10">
        <section className="flex flex-row w-full h-full gap-2">
          {Object.entries(ESTADOS).map(([estado, nombre]) => (
            <ColumnTask key={estado}>
              <TitleColumn name={nombre} tasks={tasks.filter(task => task.estado === Number(estado))} />
              <GroupTask onOpenModal={() => handleOpenModalCreate(Number(estado))}>
                {tasks
                  .filter((task) => task.estado === Number(estado))
                  .map((task) => (
                    <TaskCard>
                      <section>
                        <CardHeader>
                          <h2 className="text-base font semibold text-text-light">{task.nombre}</h2>
                          <CardActions
                            idTask={task.id}
                            idUsuarioAsignado={task.idUsuarioAsignado}
                            state={task.estado}
                            onDeleteTask={handleDeleteTask}
                            onUpdateTask={(state: number, id: number) => {
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
                        </CardHeader>
                        <CardBody>
                          <p className="text-sm font-normal text-text-muted">{task.descripcion}</p>
                        </CardBody>
                      </section>
                      <CardFooter>
                        <span className="text-xs font-normal text-primary-medium">{task.idUsuarioAsignado}</span>
                        <span className={`rounded-full w-4 h-4 ${task.color}`}></span>
                      </CardFooter>
                    </TaskCard>
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
