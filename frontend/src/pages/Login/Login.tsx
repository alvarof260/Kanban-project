import { WrapperForm, LoginForm } from "./components";

export const Login = () => {
  return (
    <div className='bg-background-primary w-screen h-screen flex justify-center items-center'>
      <WrapperForm>
        <LoginForm />
      </WrapperForm>
    </div>
  );
};
