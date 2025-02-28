import { useEffect, useState } from "react";
import { useParams } from "react-router";
import { CardBody, CardFooter, CardHeader, CustomModal, Toast } from "../../components";
import { ESTADOS } from "../../constants";
import { Board, Task } from "../../models";
import { ColumnTask, TitleColumn, GroupTask, TaskCard, FormTask, FormTaskCreate, TaskUpdateValues, CardActions } from "./components";
import { AssignTaskForm, AssignTaskValues } from "./components/AssignTaskForm/AssignTaskForm";
import { useFetch } from "../../hooks";
import { useSessionContext } from "../../contexts/session.context";

export const BoardKanban = () => {
  const { idBoard } = useParams();
  const { user } = useSessionContext();
  const { data: tasks, setData: setTasks } = useFetch<Task>(`http://localhost:5093/api/Task/GetTasksByBoardId/${idBoard}`);
  const [isOpen, setIsOpen] = useState<"create" | "edit" | "assign" | "delete" | "none">("none");
  const [infoActions, setInfoActions] = useState<{ estado: number, id: number, idUsuarioAsignado: number, nombreUsuarioAsignado: string }>({
    estado: 0,
    id: 0,
    idUsuarioAsignado: 0,
    nombreUsuarioAsignado: ""
  });
  const [board, setBoard] = useState<Board | null>(null);

  useEffect(() => {
    const fetchBoard = async () => {
      const options: RequestInit = {
        method: "GET",
        headers: { "Content-Type": "applications/json" },
        credentials: "include"
      };
      try {
        const response = await fetch(`http://localhost:5093/api/Board/getByBoardId/${idBoard}`, options);

        if (!response.ok) {
          throw new Error("Error al conectar con el servidor.");
        }

        const data: { success: boolean, data: Board } = await response.json();

        if (!data.success) {
          return;
        }

        setBoard(data.data);
      } catch (err) {
        console.error(err);
      }
    };
    fetchBoard();
  }, [idBoard]);

  console.log(tasks);

  const handleDeleteTask = async (id: number) => {
    const options: RequestInit = {
      method: "DELETE",
      credentials: "include"
    };

    try {
      const response = await fetch(`http://localhost:5093/api/Task/${id}`, options);

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
          name: newState.name !== "" ? newState.name : task.name,
          description: newState.description !== "" ? newState.description : task.description,
          color: newState.color !== "" ? newState.color : task.color,
          status: newState.status !== task.status ? newState.status : task.status
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
        ? { ...task, assignedUserId: newState.assignedUserId, assignedUserName: newState.assignedUserName }
        : task);
    setTasks(newTasks);
    setIsOpen("none");
  };

  const isOwnerBoard = board?.ownerUserName === user?.username;

  return (
    <>
      <main className="w-screen h-screen bg-background-primary p-10">
        <section className="flex flex-row w-full h-full gap-2">
          {Object.entries(ESTADOS).map(([estado, nombre]) => (
            <ColumnTask key={estado}>
              <TitleColumn name={nombre} tasks={tasks.filter(task => task.status === Number(estado))} />
              <GroupTask isOwnerBoard={isOwnerBoard} onOpenModal={() => handleOpenModalCreate(Number(estado))}>
                {tasks
                  .filter((task) => task.status === Number(estado))
                  .map((task) => (
                    <TaskCard key={task.id}>
                      <section>
                        <CardHeader>
                          <h2 className="text-base font semibold text-text-light">{task.name}</h2>
                          {
                            (isOwnerBoard || user?.id === task.assignedUserId || user?.roleUser === 1) &&
                            <CardActions
                              idTask={task.id}
                              idUsuarioAsignado={task.assignedUserId}
                              state={task.status}
                              nombreUsuarioAsignado={task.assignedUserName}
                              isOwnerBoard={isOwnerBoard}
                              onDeleteTask={handleDeleteTask}
                              onUpdateTask={(state: number, id: number) => {
                                setIsOpen("edit");
                                const newState = { ...infoActions, id, estado: state };
                                setInfoActions(newState);
                              }}
                              onAssignTask={(id: number, idUsuarioAsignado: number, nombreUsuarioAsignado: string) => {
                                setIsOpen("assign");
                                const newState = { ...infoActions, idUsuarioAsignado, id, nombreUsuarioAsignado };
                                setInfoActions(newState);
                              }}
                            />
                          }
                        </CardHeader>
                        <CardBody>
                          <p className="text-sm font-normal text-text-muted">{task.description}</p>
                        </CardBody>
                      </section>
                      <CardFooter>
                        <span className="text-xs font-normal text-primary-medium">{task.assignedUserName !== "" ? `Tarea asignada: ${task.assignedUserName}` : "Tarea no asignada"}</span>
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
          <FormTask stateInitial={infoActions.estado} idTask={infoActions.id} isOwnerBoard={isOwnerBoard} onUpdateTask={handleUpdateTask} />
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
          <AssignTaskForm idUsuarioAsignado={infoActions.idUsuarioAsignado} nombreUsuarioAsignado={infoActions.nombreUsuarioAsignado} idTask={infoActions.id} onAssignTask={handleAssignTask} />
        </CustomModal>
      }
    </>
  );
};
