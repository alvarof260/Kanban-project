import { LoginForm } from "./components";


export const Login = () => {

  return (
    <>
      <main className="w-screen h-screen bg-bg-100 flex flex-col justify-center items-center">
        <section className="w-max h-max bg-bg-200 rounded-md p-4">
          <LoginForm />
        </section>
      </main>
    </>
  );
};
