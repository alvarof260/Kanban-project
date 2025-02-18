import { useEffect, useState } from "react";
import { useParams } from "react-router";
import { CustomModal } from "../../components";
import { ESTADOS } from "../../constants";
import { Task } from "../../models";
import { ColumnTask, TitleColumn, GroupTask, TaskCard, FormTask, FormTaskCreate } from "./components";

export const Board = () => {
  const { idBoard } = useParams();
  const [tasks, setTasks] = useState<Task[]>([]);
  const [isEditing, setIsEditing] = useState<boolean>(false);
  const [stateSelected, setStateSelected] = useState<number>(0);
  const [idSelected, setIdSelected] = useState<number>(0);
  const [isCreating, setIsCreating] = useState<boolean>(false);

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

      const newTasks = tasks.filter((task) => task.idTarea !== id);

      setTasks(newTasks);
    } catch (err) {
      console.error(err);
    }
  };

  const handleUpdateTask = (newState: number, id: number) => {
    setTasks(tasks.map((task) =>
      task.idTarea === id ? { ...task, estado: newState } : task
    ));
    setIsEditing(!isEditing);
  };

  const handleOpenModalCreate = (state: number) => {
    setIsCreating(!isCreating);
    setStateSelected(state);
  };

  const handleCreateTask = (newTask: Task) => {
    setTasks([...tasks, newTask]);
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
                      key={task.idTarea}
                      nombre={task.nombre}
                      id={task.idTarea}
                      state={task.estado}
                      onDeleteTask={handleDeleteTask}
                      onChangeState={(state: number, id: number) => {
                        setIsEditing(!isEditing);
                        console.log(state);
                        setStateSelected(state);
                        setIdSelected(id);
                      }}
                    />
                  ))}
              </GroupTask>
            </ColumnTask>
          ))}
        </section>
      </main>
      {
        isEditing &&
        <CustomModal onModal={() => setIsEditing(!isEditing)}>
          <FormTask stateInitial={stateSelected} idTask={idSelected} onUpdateTask={handleUpdateTask} />
        </CustomModal>
      }
      {
        (isCreating && idBoard) &&
        <CustomModal onModal={() => setIsCreating(!isCreating)}>
          <FormTaskCreate state={stateSelected} idBoard={Number(idBoard)} onAddTask={handleCreateTask} />
        </CustomModal>
      }
    </>
  );
};
