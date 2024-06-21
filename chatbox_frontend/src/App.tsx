import { Button } from "./components/ui/button";
import { ThemeProvider } from "./components/ui/theme-provider";
import { ModeToggle } from "./components/ui/mode-toggle";
function App() {
  return (
    <ThemeProvider>
      <div>
        <h1 className="text-3xl">Chatbox</h1>
        <p>
          <ModeToggle />
          Chatbox is a simple chat application built with React, ASP.NET, and
          SignalR.
          <Button>Click me</Button>
        </p>
      </div>
    </ThemeProvider>
  );
}

export default App;
