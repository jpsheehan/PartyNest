from yaml import load, dump


def main():
    with open("out.cs", "w") as f:
        data = load(open("parrots.yaml"))
        for parrot in data:
            hd = "false"
            tip = ""
            path = ""
            if "hd" in parrot:
                hd = "true"
                path = parrot["hd"]
            else:
                path = parrot["gif"]
            if "tip" in parrot:
                tip = parrot["tip"]
            name = parrot["name"]
            try:
                f.write(
                    "new Parrot {{ Name = \"{0}\", Path = \"{1}\", IsHD = {2}, Tip = \"{3}\", }},\n".format(name, path, hd, tip))
            except:
                print("skipping", name)


if __name__ == "__main__":
    main()
